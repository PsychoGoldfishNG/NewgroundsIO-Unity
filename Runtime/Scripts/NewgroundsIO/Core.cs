using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace NewgroundsIO {

	/// <summary>A delegate used to pass results on server response events.</summary>
	public delegate void ServerResponseDelegate(NewgroundsIO.objects.Response response);

	/// <summary>A class that handles all Newgrounds.io server communication and object serialization/deserialization.</summary>
	public class Core
	{
		/** ================================ Constants =============================== **/

		/// <summary>The URI to v3 of the Newgrounds.io gateway.</summary>
		const string GATEWAY_URI = "https://www.newgrounds.io/gateway_v3.php";


		/** ============================== Public Values ============================= **/

		/// <summary>An event to be dispatched whenever the server responds to a call.</summary>
		public event ServerResponseDelegate ServerResponse;
		
		/// <summary>A list of pending components to be called via ExecuteQueue.</summary>
		public List<NewgroundsIO.BaseComponent> componentQueue = new List<NewgroundsIO.BaseComponent>();

		/// <summary>An index of any parameters set in the current URL's query string.</summary>
		public Dictionary<string,string> uriParams = new Dictionary<string,string>();

		
		/** ============================ Public Properties =========================== **/

		/// <summary>The App ID from your App Settings page.</summary>
		public string appID { get; private set; }

		/// <summary>Set to true to enable debug mode.</summary>
		public bool debug {get; set;} = false;

		/// <summary>Returns the host domain in WebGL builds.</summary>
		public string host { get {
			this.ParseURL();
			return this._host;
		}}

		/// <summary>Returns true if any components are in the execute queue.</summary>
		public bool hasQueue { get { return componentQueue.Count > 0; }}

		/// <summary>The user session object.</summary>
		public NewgroundsIO.objects.Session session {get; private set;} = new NewgroundsIO.objects.Session();

		/// <summary>The active user object.</summary>
		public NewgroundsIO.objects.User user {get { return this.session.user; }}


		/** ============================= Private Values ============================= **/

		// AES-128 key converted from Base64 to binary.
		private byte[] aesKey;

		// Holds any echo text to be attached to the next component/queue execution.
		private string _echo = null;

		// Holds the host domain in WebGL Builds.
		private string _host = null;


		/** ============================= Public Methods ============================= **/

		/// <summary>Constructor</summary>
		/// <param name="appID">The App ID from your Newgrounds project App Settings page.</param>
		/// <param name="aesKey">The AES-128 encryption key from your Newgrounds project App Settings page.</param>
		public Core(string appID, string aesKey)
		{
			this.appID = appID;
			this.aesKey = Convert.FromBase64String(aesKey);
			this.session.SetCore(this);
		}

		/// <summary>Used to attach a string that will be echo'd back in the Response object from the next component or queue execution.</summary>
		/// <param name="string">The string you want the server to return.</param>
		public void SetEcho(string echo) {
			_echo = echo;
		}

		/// <summary>Gets a query parameter value from the URI hosting this game.</summary>
		/// <param name="param">The parameter you want to get a value for.</param>
		public string GetUriParam(string param)
		{
			this.ParseURL();
			return this.uriParams.ContainsKey(param) ? this.uriParams[param] : null;
		}

		/// <summary>Adds a component to the execution queue (use ExecuteQueue to complete).</summary>
		/// <param name="component">The component object to queue</param>
		public void QueueComponent(NewgroundsIO.BaseComponent component)
		{
			// redirect components can't be called
			if (component.__properties.Contains("redirect")) {
				bool _redir = (bool)component.GetType().GetProperty("redirect").GetValue(component, null);
				if (_redir) {
					Debug.LogWarning("NewgroundsIO - You can not queue redirects!");
					return;
				}
			}
			
			ApplyHost(component);
			component.SetCore(this);

			if (!component.IsValid()) {
				Debug.LogError("NewgroundsIO Error: INVALID COMPONENT!");
				return;
			}

			componentQueue.Add(component);
		}

		/// <summary>Executes a component in a new browser tab. Typically used for Loader components.</summary>
		/// <param name="component">The component you want to load.</param>
		public void LoadComponent(NewgroundsIO.BaseComponent component)
		{
			if (!component.__object.StartsWith("Loader.")) {
				Debug.LogWarning("NewgroundsIO - Only Loader components can be used with Core.LoadComponent");
				return;
			}

			bool redirect = (bool)component.GetType().GetProperty("redirect").GetValue(component, null);
			if (!redirect) {
				Debug.LogWarning("NewgroundsIO - component.redirect can't be false when used with Core.LoadComponent");
				component.GetType().GetProperty("redirect").SetValue(component, true, null);
			}

			ApplyHost(component);
			component.SetCore(this);

			if (!component.IsValid()) {
				Debug.LogError("NewgroundsIO Error: INVALID COMPONENT!");
				return;
			}

			var request = new NewgroundsIO.objects.Request();
			request.isList = false;
			request.debug = this.debug;
			request.execute = new NewgroundsIO.ExecuteWrapper();
			request.execute.component = component;
			request.SetCore(this);
			request.RequiresSession(component.__requireSession);

			Application.OpenURL(GATEWAY_URI+"?input="+HttpUtility.UrlEncode(request.ToJSON()));
		}

		/// <summary>Encrypts a JSON-encoded string and encodes it to a base64 string.</summary>
		/// <param name="jsonString">The encoded JSON string to encrypt.</param>
		public string Encrypt(string jsonString)
		{
			RijndaelManaged myRijndael = new RijndaelManaged();
			myRijndael.Key = this.aesKey;
			myRijndael.GenerateIV();
			byte[] aes = EncryptAES128(jsonString, myRijndael.Key, myRijndael.IV);

			byte[] encrypted_bytes = new byte[myRijndael.IV.Length + aes.Length];
			Buffer.BlockCopy(myRijndael.IV, 0, encrypted_bytes, 0, myRijndael.IV.Length);
			Buffer.BlockCopy(aes, 0, encrypted_bytes, myRijndael.IV.Length, aes.Length);

			return Convert.ToBase64String(encrypted_bytes);
		}

		/// <summary>Encrypts a string to an AES128 byte array.</summary>
		/// <param name="plainText">The string you want to encode.</param>
		/// <param name="Key">The encryption key.</param>
		/// <param name="IV">The IV to use during encryption.</param>
		internal static byte[] EncryptAES128(string plainText, byte[] Key, byte[] IV)
		{
			// Check arguments. 
			if (plainText == null || plainText.Length <= 0)
				throw new ArgumentNullException("plainText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");
			byte[] encrypted;

			// Create an RijndaelManaged object 
			// with the specified key and IV. 
			using (RijndaelManaged rijAlg = new RijndaelManaged())
			{
				rijAlg.Key = Key;
				rijAlg.IV = IV;

				// Create a decryptor to perform the stream transform.
				ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for encryption. 
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{

							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}


			// Return the encrypted bytes from the memory stream. 
			return encrypted;

		}

		/// <summary>Invokes any ServerResponse delegates attached to the core</summary>
		/// <param name="response">The response object returned by the server</param>
		protected virtual void OnServerResponse(NewgroundsIO.objects.Response response)
		{
			ServerResponse?.Invoke(response);
		}


		/** ============================ Public Coroutines =========================== **/

		/// <summary>Executes a component on the server, then passes the response to a callback function.</summary>
		/// <param name="component">The component object you want to execute.</param>
		/// <param name="callback">The function to call when the server responds.</param>
		public IEnumerator ExecuteComponent(NewgroundsIO.BaseComponent component, Action<NewgroundsIO.objects.Response> callback=null)
		{
			ApplyHost(component);
			if (!component.IsValid()) {
				Debug.LogError("NewgroundsIO Error: INVALID COMPONENT!");
				yield break;
			}

			var request = new NewgroundsIO.objects.Request();
			request.isList = false;
			request.debug = this.debug;
			request.execute = new NewgroundsIO.ExecuteWrapper();
			request.execute.component = component;
			request.echo = _echo;
			_echo = null;
			request.SetCore(this);
			request.RequiresSession(component.__requireSession);

			yield return this._doExecuteComponent(request.ToJSON(), callback);
		}

		/// <summary>Executes a list of queued component objects, then passes the response to a callback function.</summary>
		/// <param name="callback">The function to call when the server responds.</param>
		public IEnumerator ExecuteQueue(Action<NewgroundsIO.objects.Response> callback=null)
		{
			if (this.componentQueue.Count == 0) {
				Debug.LogWarning("NewgroundsIO - Attempted to send empty component queue.");
				yield break;
			}

			var request = new NewgroundsIO.objects.Request();
			request.isList = true;
			request.debug = this.debug;
			request.echo = _echo;
			_echo = null;
			request.SetCore(this);

			this.componentQueue.ForEach(component => {
				var execute = new NewgroundsIO.ExecuteWrapper();
				execute.SetCore(this);
				execute.component = component;
				request.executeList.Add(execute);
				if (component.__requireSession) request.RequiresSession(true);
			});

			this.componentQueue = new List<NewgroundsIO.BaseComponent>();

			yield return this._doExecuteComponent(request.ToJSON(), callback);
		}


		/** =========================== Private Coroutines =========================== **/

		// handles the actual execution of components
		private IEnumerator _doExecuteComponent(string jsonOut, Action<NewgroundsIO.objects.Response> callback=null)
		{
			if (debug) Debug.Log("OUTPUT >>>>");
			if (debug) Debug.Log(jsonOut);

			// post the JSON object to the gateway
			WWWForm form = new WWWForm();
			form.AddField("input", jsonOut);
			UnityWebRequest post = UnityWebRequest.Post(GATEWAY_URI, form);
			yield return post.SendWebRequest();

			// create the response container
			NewgroundsIO.objects.Response responseObject = new NewgroundsIO.objects.Response();

			// there was a problem sending the request...
			if (post.result == UnityWebRequest.Result.ConnectionError)
			{
				Debug.LogError("Error While Sending: " + post.error);
				if (!(callback is null)) callback(null);
			}

			else
			{
				try {

					// convert the JSON response to an object
					var response = JsonConvert.DeserializeObject<object>(post.downloadHandler.text);
					if (debug) {
						Debug.Log("INPUT >>>");
						Debug.Log(response);
					}

					// import the object into our response container, and pass reference to this core
					responseObject.FromJSON(response);
					responseObject.SetCore(this);

					// invoke any delegates.
					OnServerResponse(responseObject);

					// fire the callback function
					if (!(callback is null)) callback(responseObject);
				}

				// handle any errors in decoding the JSON response
				catch(JsonException e)
				{
					Debug.LogError(e);
					if (!(callback is null)) callback(null);
				}
			}

			// clean up the mess
			post.Dispose();
		}


		/** ============================= Private Methods ============================ **/

		// Parses the query string from the URI hostin this game.		
		private void ParseURL()
		{
			if (!(this._host is null)) return;

			string web_url = Application.absoluteURL;
			
			if (string.IsNullOrEmpty(web_url)) {
			
				this._host = "<AppView>";
			
			} else if (web_url.StartsWith("file")) {

				this._host = "<LocalHost>";

			} else if (web_url.StartsWith("http")) {

				Uri uri = new Uri(web_url);
				this._host = uri.Host;
				
				if (string.IsNullOrEmpty(this.host)) {
					this._host = "<Unknown>";
				} else {
					string query = uri.Query;
					
					if (!string.IsNullOrEmpty(query)) {

						if (query.Substring(0,1) == "?") query = query.Remove(0,1);
						var pairs = query.Split("&");
						foreach(var pair in pairs)
						{
							var key_val = pair.Split("=");
							this.uriParams.Add(key_val[0], key_val[1] ?? null);
						}
					}
				}

			}

			this.session.SetCore(this);
			
			if (!(this.GetUriParam("ngio_session_id") is null)) {
				this.session._uri_id = this.GetUriParam("ngio_session_id");
			}
		}

		// apply the host domain, or other indicator to any components that need it
		private void ApplyHost(NewgroundsIO.BaseComponent component)
		{
			if (!(this.host is null) && component.__properties.Contains("host")) {
				component.host = this.host;
			}
		}
	}


	/** ============================== Base Object Class ============================= **/

	/// <summary>A base for every object, component and response.</summary>
	public class BaseObject {

		/// <summary>Used to link a Core object.</summary>
		public Core __ngioCore = null;

		/// <summary>Used to tell serialization that this is an object model.</summary>
		public string type = "object";

		/// <summary>Used to identify what type of object this is.</summary>
		public string __object = "base";

		/// <summary>Contains a list of serializable properties.</summary>
		public List<string> __properties = new List<string>();

		/// <summary>Contains a list of required properties.</summary>
		public List<string> __required = new List<string>();

		/// <summary>Contains a list of properties that can be returned as arrays.</summary>
		public List<string> __objectArrays = new List<string>();

		/// <summary>Contains a map of properties that should be deserialized as an object model.</summary>
		public Dictionary<string,string> __objectMap = new Dictionary<string,string>();

		/// <summary>Contains a set of numeric datatypes we can convert to JSON numbers.</summary>
		private HashSet<Type> _numberTypes = new HashSet<Type> {
			typeof(int),
			typeof(short),
			typeof(ushort),
			typeof(uint),
			typeof(float),
			typeof(double),
			typeof(decimal)
		};

		/// <summary>Some objects have list-based properties. Override this to handle those as needed.</summary>
		/// <param name="propName">The name of the property.</param>
		/// <param name="obj">The object to be added to a list.</param>
		public virtual void AddToPropertyList(string propName, NewgroundsIO.BaseObject obj) {}

		/// <summary>Some object properties need custom processing. Override this to handle those as needed.</summary>
		/// <param name="propName">The name of the property.</param>
		/// <param name="jsonObj">A JObject waiting to be deserialized.</param>
		public virtual bool SetPropertyCustom(string propName, JObject jsonObj) {
			return false;
		}

		/// <summary>Recursively links a core instance.</summary>
		/// <param name="ngio">The core instance to link to this object.</param>
		public virtual void SetCore(Core ngio) 
		{
			this.__ngioCore = ngio;

			this.__properties.ForEach(prop => {
				
				var val = GetPropValue(this, prop);
				
				if ((val is BaseObject)) {
					BaseObject _obj = val as BaseObject;
					if (_obj.__ngioCore is null) _obj.SetCore(ngio);
				}
			});

			// some classes will have lists of objects that will need the core assigned
			this.SetCoreOnLists(ngio);
		}

		/// <summary>Override this to handle linking the core to objects contained in lists or dictionaries</summary>
		public virtual void SetCoreOnLists( NewgroundsIO.Core ngio ) {}

		/// <summary>Returns true if all required properties are set.</summary>
		public virtual bool IsValid() {
			if (this.__required.Count == 0) return true;

			bool valid = true;

			this.__required.ForEach(req => {
				if (GetPropValue(this, req) is null) {
					Debug.LogError("NewgroundsIO Error - Missing required property: "+req+" in "+this);
				}
			});
			return valid;
		}

		/// <summary>Extracts the value of a property from an object by name.</summary>
		/// <param name="obj">The object you are extracting from.</param>
		/// <param name="prop">The name of the property to extract.</param>
		public static object GetPropValue(object obj, string prop)
		{
			return obj.GetType().GetProperty(prop)?.GetValue(obj, null);
		}		

		/// <summary>Serializes this object to a JSON string</summary>
		public string ToJSON()
		{
			string jsonOut = "{";

			bool first = true;

			this.__properties.ForEach(propName => {

				if (this._skipJsonProp(propName)) return;

				if (first) {
					first = false;
				} else {
					jsonOut += ",";
				}
				jsonOut += this._getPropertyJSON(propName);

			});

			jsonOut += "}";

			return jsonOut;
		}

		/// <summary>Processes deserialized JSON object to fill this object's properties</summary>
		public void FromJSON(object json) 
		{
			// make sure this is an actual JObject
			if (json is JObject) {

				var jsonObj = json as JObject;

				// go through each of this objects properties, and see if we can extract a value from the JObject
				this.__properties.ForEach(propName => {
					
					var prop = this.GetType().GetProperty(propName);

					// skip missing or null properties
					if (prop is null || jsonObj.GetValue(propName) is null) {
						return;
					}

					try {

						// try getting a generic JObject from this value (should only work for things like lists and dictionaries)
						JObject cObj = null;
						try {
							cObj = (JObject)jsonObj.GetValue(propName).ToObject(typeof(JObject));
						}
						catch(JsonSerializationException) {
							cObj = null;
						}

						// If there's no custom property handler, use base values or object mapping
						if ((cObj is null) || !this.SetPropertyCustom(propName, cObj)) {

							switch (jsonObj.GetValue(propName).Type.ToString()) {

								case "Null":
									prop.SetValue(this, null, null);
									break;

								case "Boolean":
									var boolVal = (bool)jsonObj.GetValue(propName).ToObject(typeof(bool));
									prop.SetValue(this, boolVal, null);
									break;

								case "Date":
									var dateVal = (string)jsonObj.GetValue(propName).ToString(Formatting.None).Trim('"');
									prop.SetValue(this, dateVal, null);
									break;

								case "String":
									var stringVal = (string)jsonObj.GetValue(propName).ToObject(typeof(string));

									// some legacy stuff still sends numbers as string
									// try it as an actual string first
									try {
										prop.SetValue(this, stringVal, null);
									}
									// fall back to a number
									catch (ArgumentException) {
										try {
											// this will be a float
											if (stringVal.Contains(".")) {
												prop.SetValue(this, float.Parse(stringVal), null);
											// this is probably an integer
											} else {
												prop.SetValue(this, int.Parse(stringVal), null);
											}
										}
										catch (ArgumentException) {
											Debug.LogError("NewgroundsIO Error: Unexpected String value for "+this+" -> "+propName);
										}
									}
									break;

								case "Integer":
									var intVal = (int)jsonObj.GetValue(propName).ToObject(typeof(int));

									// some legacy stuff still uses 0/1 as a shortform for booleans
									// try it as an actual integer first
									try {
										prop.SetValue(this, intVal, null);
									}
									// fall back to a boolean
									catch (ArgumentException) {
										try {
											var intBool = intVal == 0 ? false:true;
											prop.SetValue(this, intBool, null);
										}
										catch (ArgumentException) {
											Debug.LogError("NewgroundsIO Error: Unexpected Integer value for "+this+" -> "+propName);
										}
									}
									break;

								case "Float":
									var floatVal = (float)jsonObj.GetValue(propName).ToObject(typeof(float));
									prop.SetValue(this, floatVal, null);
									break;

								case "Array":

									var jArr = (JArray)jsonObj.GetValue(propName).ToObject(typeof(JArray));

									// The response object needs to figure out the appropriate result object array to use
									if (propName == "result" && this is NewgroundsIO.objects.Response) {
										var _response = this as NewgroundsIO.objects.Response;
										_response.SetResultsList(jArr);
										break;
									}

									// Everything else should have an object map
									if (this.__objectMap.ContainsKey(propName)) {
										foreach (JObject jObj in jArr) {
											this.AddToPropertyList( propName, NewgroundsIO.ObjectIndex.CreateObject(this.__objectMap[propName], jObj) );
										}
									}

									break;

								case "Object":

									// The response object has special handling for results, wich could be many different things
									if (propName == "result" && this is NewgroundsIO.objects.Response) {
										var _response = this as NewgroundsIO.objects.Response;
										_response.SetResults((JObject)jsonObj.GetValue("result").ToObject(typeof(JObject)));
										break;
									}

									// If this property is in our object map, we can cast it from the ObjectIndex
									if (this.__objectMap.ContainsKey(propName)) {
										prop.SetValue(this, NewgroundsIO.ObjectIndex.CreateObject(this.__objectMap[propName], cObj), null);
									}

									break;

								default:

									Debug.LogWarning("Unknown Value! "+propName+" "+jsonObj.GetValue(propName).Type.ToString());
									break;
							}
						}
					}

					catch (ArgumentException) {
						Debug.LogError("NewgroundsIO Error: Unexpected "+jsonObj.GetValue(propName).Type.ToString()+" value for "+this+" -> "+propName);
					}

				});
			}
		}

		/// <summary>
		/// When serializing, properties that are null, and not required will be left out of the JSON string.
		/// This checks a property (by name) to see if it can be skipped
		///	</summary>
		/// <param name="propName">The name of the property to check.</param>
		public virtual bool _skipJsonProp(string propName)
		{
			bool required = this.__required.Contains(propName);
			if (!required && GetPropValue(this, propName) is null) return true;

			return false;
		}

		/// <summary>Encodes a property into a JSON string.</summary>
		/// <param name="propName">The name of the property to encode.</param>
		public virtual string _getPropertyJSON(string propName) 
		{
			return "\""+propName+"\":"+this._getValueJSON(GetPropValue(this, propName));
		}

		/// <summary>Encodes a value into JSON string.</summary>
		/// <param name="val">Any value you need to encode.</param>
		public string _getValueJSON(object val)
		{
			// simple null string
			if (val is null) return "null";

			// object models 
			if (val is NewgroundsIO.BaseObject) {

				var objVal = val as NewgroundsIO.BaseObject;
				return objVal.ToJSON();

			// booleans
			} else if (val is bool) {

				return (bool) val ? "true":"false";

			// strings
			} else if (val is string) {

				string stringVal = val as string;
				return "\""+stringVal.Replace("\"", "\\\"")+"\"";

			// lists
			} else if (val.GetType().IsGenericType && val is IEnumerable) {

				string _out = "[";
				bool _first = true;
				var _enum = ((IEnumerable) val).GetEnumerator();
				while (_enum.MoveNext()) {
					
					if (!_first) {
						_out += ",";
					} else {
						_first = false;
					}
					_out += _getValueJSON(_enum.Current);
				}
				_out += "]";
				return _out;

			// numbers
			} else if (this._numberTypes.Contains(val.GetType())) {

				return val.ToString();

			}

			// we don't use any other types, so shoot a warning and return a null string
			Debug.LogWarning("NewgroundsIO: Unsupported type: "+val.GetType());
			return "null";
		}
	}


	/** ============================ Base Component Class ============================ **/

	/// <summary>A base for every component model.</summary>
	public class BaseComponent : NewgroundsIO.BaseObject {

		/// <summary>Components that require encryption will set this to true</summary>
		public bool __isSecure = false;

		/// <summary>Components that require a session ID will set this to true</summary>
		public bool __requireSession = false;

		/// <summary>Many components need to pass the hosting website (or indicator this is a standalone app).</summary>
		public string host { get; set; }

		/// <summary>All components have an optional echo property that will return the same value in a server response.</summary>
		public string echo { get; set; } = null;

		/// <summary>Constructor</summary>
		public BaseComponent() {

			// add echo to the property list
			this.__properties.Add("echo");
		}
	}


	/** ============================= Base Result Class ============================== **/

	/// <summary>A base for every result model.</summary>
	public class BaseResult : NewgroundsIO.BaseObject {

		/// <summary>If the component was successful, this will be true. If not, you can check the error property for details.</summary>
		public bool success { get; set; } = false;

		/// <summary>If there was an error with the component, this will be an Error object.</summary>
		public NewgroundsIO.objects.Error error { get; set; } = null;

		/// <summary>The name of the component that was called to yield this result.</summary>
		public string component { get { return this.__object; } }

		/// <summary>All components can send an echo string. They will be returned in this property.</summary>
		public string echo { get; set; } = null;

		/// <summary>Constructor</summary>
		public BaseResult() {
			this.__properties.Add("echo");
			this.__properties.Add("error");
			this.__properties.Add("success");
			this.__objectMap.Add("error","Error");
		}
	}


	/** =============================== Execute Object =============================== **/

	/// <summary>
	/// This object is a wrapper for serializing components using their string name, and property objects.
	/// </summary>
	public class ExecuteWrapper : NewgroundsIO.BaseObject {

		/// <summary>The component object we're wrapping</summary>
		public NewgroundsIO.BaseComponent component { get; set; } = null;

		public ExecuteWrapper() {
			// add component to the property and required lists
			this.__properties.Add("component");
			this.__required.Add("component");
		}

		/// <summary>
		/// This overrides the default encoder to add the conponent's name and properties as separate values.
		/// Will also handle encrypting secure components
		/// </summary>
		/// <param name="propName"></param>
		public override string _getPropertyJSON(string propName) 
		{
			// this is the override for encoding the 'component' property
			if (propName == "component") {

				// null components are bad
				if (this.component is null) {
					Debug.LogError("NewgroundsIO Error: Missing component object!");
					return "\"component\":null";
				}

				// create the JSON string for this component
				string jsonOut = "\"component\":\""+this.component.__object+"\",\"parameters\":"+this.component.ToJSON();

				// if this is a secure component, encrypt it and return it as a 'secure' JSON string
				if (component.__isSecure) {
					return "\"secure\":\""+this.__ngioCore.Encrypt("{"+jsonOut+"}")+"\"";
				}

				// otherwise, return the plain JSON string
				return jsonOut;

			}

			// anything else uses the default encoder
			return base._getPropertyJSON(propName);
		}
	}


	/** =============================== Session States =============================== **/

	/// <summary>
	/// Contains a bunch of constants representing the different states a user session can be in.
	/// This is used by the NewgroundsIO.objects.Session object
	/// </summary>
	public static class SessionState {

		public const string SESSION_UNINITIALIZED		= "session-uninitialized";	// We have never checked this session
		public const string WAITING_FOR_SERVER			= "waiting-for-server";		// We are waiting for the server to send information
		public const string LOGIN_REQUIRED				= "login-required";			// We have a session, but the user isn't logged in
		public const string WAITING_FOR_USER			= "waiting-for-user";		// The user has opened the login page
		public const string LOGIN_CANCELLED				= "login-cancelled";		// The user cancelled the login
		public const string LOGIN_SUCCESSFUL			= "login-successful";		// The user is logged in, session is valid!
		public const string LOGIN_FAILED				= "login-failed";			// The user failed their login attempt
		public const string USER_LOGGED_OUT				= "user-logged-out";		// The user logged out
		public const string SERVER_UNAVAILABLE			= "server-unavailable";		// The server is currently unavailable
		public const string EXCEEDED_MAX_ATTEMPTS		= "exceeded-max-attempts";	// We've failed trying to connect too many times

		/// <summary>States in this list are considered "waiting". You don't need to make any API calls during these.</summary>
		public static List<string> SESSION_WAITING = new List<string>{ 
			WAITING_FOR_SERVER, 
			WAITING_FOR_USER,
			LOGIN_CANCELLED,
			LOGIN_FAILED
		};
	}
}
