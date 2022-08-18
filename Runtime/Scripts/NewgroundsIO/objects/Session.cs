using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains information about the current user session.</summary>
	public class Session : NewgroundsIO.BaseObject {

		/// <summary>A unique session identifier</summary>
		public string id { get; set; }

		/// <summary>If the user has not signed in, or granted access to your app, this will be null</summary>
		public NewgroundsIO.objects.User user { get; set; }

		/// <summary>If true, the session_id is expired. Use App.startSession to get a new one.</summary>
		public bool expired { get; set; }

		/// <summary>If true, the user would like you to remember their session id.</summary>
		public bool remember { get; set; }

		/// <summary>If the session has no associated user but is not expired, this property will provide a URL that can be used to sign the user in.</summary>
		public string passport_url { get; set; }

		/// <summary>The current state of this session.</summary>
		public string status { get; private set; } = NewgroundsIO.SessionState.SESSION_UNINITIALIZED;

		/// <summary>Stores a session ID from the game's URI if hosted on Newgrounds.</summary>
		public string _uri_id = null;

		/// <summary>Stores a session ID that was saved from a Passport login.</summary>
		public string _saved_id = null;

		/// <summary>This will return true if the status has changed since the last time Update() was called.</summary>
		public bool statusChanged { get { return (this._lastStatus != this.status); } }

		/// <summary>Returns true if the current state is a waiting state.</summary>
		public bool waiting { get {
			return NewgroundsIO.SessionState.SESSION_WAITING.Contains(this.status);
		} }

		// The last time Update() was called.
		private DateTime _lastUpdate;

		// If false, Update() will end immediately when called.
		private bool _canUpdate = true;

		// The status from the last time Update() was called.
		private string _lastStatus = null;

		// The mode we'll use to check the status of this session.
		private string mode = "expired";

		// The total number of attempts we've tried to contact the server without success.
		private ushort _totalAttempts = 0;

		// 
		// The max number of attempts we can make to the server without success before we give up.
		private ushort _maxAttempts = 5;

		/// <summary>Constructor</summary>
		public Session()
		{
			this.__object = "Session";

			this.__properties.Add("id");
			this.__properties.Add("user");
			this.__properties.Add("expired");
			this.__properties.Add("remember");
			this.__properties.Add("passport_url");
			this.__objectMap.Add("user","User");
			// set this in the past so the first time-lapse check won't make the user wait
			this._lastUpdate = DateTime.Now.AddMinutes(-30);
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Session clone(NewgroundsIO.objects.Session cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Session();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}


		// resets everything except the session id
		private void ResetSession()
		{
			this._uri_id = null;
			this._saved_id = null;
			this.remember = false;
			this.user = null;
			this.expired = false;

			PlayerPrefs.SetString("__ngio_session_id", null);
		}

		/// <summary>Opens the Newgrounds Passport login page in a new browser tab</summary>
		public void OpenLoginPage()
		{
			if (!String.IsNullOrEmpty(this.passport_url)) {
				Application.OpenURL(this.passport_url);
				this.status = NewgroundsIO.SessionState.WAITING_FOR_USER;
				this.mode = "check";
			}
		}

		/// <summary>Logs the user out of their current session, locally and on the server, then calls a function when complete.</summary>
		/// <param name="callback">The callback function.</param>
		public IEnumerator LogOut(Action<NewgroundsIO.objects.Session> callback=null)
		{
			this.mode = "wait";
			yield return this.EndSession();

			if (!(callback is null)) callback(this);
		}

		/// <summary>Cancels a pending login attempt.</summary>
		/// <param name="newStatus">An optional status code to use if LOGIN_CANCELLED is insufficient.</param>
		public void CancelLogin(string newStatus = NewgroundsIO.SessionState.LOGIN_CANCELLED)
		{
			// clear the current session data, and set the appropriate cancel status
			this.ResetSession();
			this.id = null;
			this.status = newStatus;

			// this was a manual cancel, so we can reset the retry counter
			this._totalAttempts = 0;

			// let the user pull a new session right away
			this.mode = "new";
			this._lastUpdate = DateTime.Now.AddMinutes(-30);
		}

		/// <summary>Call this to update the session process and call a function if there are any changes.</summary>
		/// <param name="callback">The callback function.</param>
		public IEnumerator Update(Action<NewgroundsIO.objects.Session> callback=null) {
		
			// if we have a new status, we can fire the callback
			if (this._lastStatus != this.status) {
				this._lastStatus = this.status;
				if (!(callback is null)) callback(this);
			}

			// we can skip this whole routine if we're in the middle of checking things
			if (!this._canUpdate || this.mode == "wait") yield break;

			// Server is not responding as expected, it may be down...  We'll set the session back to unintialized and try again
			if (this.status == NewgroundsIO.SessionState.SERVER_UNAVAILABLE) {
				
				// we've had too many failed attempts, time to stop retrying
				if (this._totalAttempts >= this._maxAttempts) {
					this.status = NewgroundsIO.SessionState.EXCEEDED_MAX_ATTEMPTS;

				// next time our delay time has passed, we'll reset this, and try our sessions again
				} else {
					this.status = NewgroundsIO.SessionState.SESSION_UNINITIALIZED;
					this._totalAttempts++;

				}
			}

			// first time getting here (probably).  We need to see if we have any existing session data to try...
			if (this.status == NewgroundsIO.SessionState.SESSION_UNINITIALIZED) {

				this._saved_id = PlayerPrefs.GetString("__ngio_session_id", null);

				// check if we have a session id from our URL params (hosted on Newgrounds)
				if (!String.IsNullOrEmpty(this._uri_id)) {
					this.id = this._uri_id;

				// check if we have a saved session (hosted elsewhere or standalone app)
				} else if (!String.IsNullOrEmpty(this._saved_id)) {
					this.id = this._saved_id;

				}

				// If we have an existing session, we'll use "check" mode to varify it, otherwise we'll nequest a "new" one.
				this.mode = String.IsNullOrEmpty(this.id) ? "new" : "check";
			
			}

			// make sure at least 5 seconds pass between each API call so we don't get blocked by DDOS protection.
			var wait = (DateTime.Now - this._lastUpdate).TotalSeconds;
			if (wait < 5) yield break;
			this._lastUpdate = DateTime.Now;
			
			switch (this.mode) {

				// we don't have an existing session, so we're requesting a new one
				case "new":

					// change our mode to wait so the coroutine can finish before we make ny other API calls
					this.mode = "wait";
					yield return this.StartSession();
					break;

				// we have a session, we just need to check and see if there's a valid login attached to it
				case "check":

					// change our mode to wait so the coroutine can finish before we make ny other API calls
					this.mode = "wait";
					yield return this.CheckSession();
					break;
			}
		}

		// =================================== API CALLS/HANDLERS =================================== //


		/** App.startSession **/

		/// <summary>This will reset our current session object, then make the API call to get a new session.</summary>
		public IEnumerator StartSession()
		{
			// don't check for any new updates while we're starting the new session
			this._canUpdate = false;
			
			// clear out any pre-existing session data
			this.ResetSession();

			this.status = NewgroundsIO.SessionState.WAITING_FOR_SERVER;

			var startSession = new NewgroundsIO.components.App.startSession();
			yield return __ngioCore.ExecuteComponent(startSession, this.OnStartSession);
		}

		// Handles the acquisition of a new session id from the server.
		protected void OnStartSession(NewgroundsIO.objects.Response response)
		{

			// The start session request was successful!
			if (response.success == true) {

				// get our component result, and grab it's session object
				var result = response.result as NewgroundsIO.results.App.startSession;
				NewgroundsIO.objects.Session newSession = result.session;
				
				// save the new session data to this session object
				this.id = newSession.id;
				this.passport_url = newSession.passport_url;

				// update our session status. This will trigger the callback in our update loop.
				this.status = NewgroundsIO.SessionState.LOGIN_REQUIRED;

				// The update loop needs to wait until the user clicks a login button
				this.mode = "wait";
				
			// Something went wrong!  (Good chance the servers are down)
			} else {
				this.status = NewgroundsIO.SessionState.SERVER_UNAVAILABLE;
			}

			// Let our update loop know it can actually do stuff again
			this._canUpdate = true;
		}


		/** App.checkSession **/

		/// <summary>This will call the API to see what the status of our current session is</summary>
		public IEnumerator CheckSession()
		{
			// don't check for any new updates while we're checking session
			this._canUpdate = false;

			var checkSession = new NewgroundsIO.components.App.checkSession();
			checkSession.SetCore(__ngioCore);
			yield return __ngioCore.ExecuteComponent(checkSession, this.OnCheckSession);
		}

		// Handles the response to CheckSession. This may lead to a change in status if the user has signed in, 
		// cancelled, or the session has expired.
		public void OnCheckSession(NewgroundsIO.objects.Response response)
		{
			// The API request was successful 
			if (response.success == true) {

				// get our component result, and grab it's session object
				var result = response.result as NewgroundsIO.results.App.checkSession;
				
				// Our session either failed, or the user cancelled the login on the server.
				if (!result.success) {

					// clear our id, and cancel the login attempt
					this.id = null;
					this.CancelLogin(result.error.code == 111 ? NewgroundsIO.SessionState.LOGIN_CANCELLED : NewgroundsIO.SessionState.LOGIN_FAILED);
					
				} else {

					// The session is expired
					if (result.session.expired) {

						// reset the session so it's like we never had one
						this.ResetSession();
						this.id = null;
						this.status = NewgroundsIO.SessionState.SESSION_UNINITIALIZED;

					// we have a valid user login attached!
					} else if (!(result.session.user is null)) {

						// store the user info, and update status
						this.user = result.session.user;
						this.status = NewgroundsIO.SessionState.LOGIN_SUCCESSFUL;
						this.mode = "valid";

						// if the user selected to remember the login, save it now!
						if (result.session.remember) {
							this._saved_id = this.id;
							this.remember = true;
							PlayerPrefs.SetString("__ngio_session_id", this.id);
						}

					// Nothing has changed, we'll have to check again in the next loop.
					} else {
						this.mode = "check";
					}
				}

			} else {

				// Something went wrong!  Servers may be down, or you got blocked for sending too many requests
				this.status = NewgroundsIO.SessionState.SERVER_UNAVAILABLE;

			}

			// Let our update loop know it can actually do stuff again
			this._canUpdate = true;
		}


		/** App.endSession **/

		/// <summary>This will end the current session on the server</summary>
		public IEnumerator EndSession()
		{
			// don't check for any new updates while we're ending session
			this._canUpdate = false;

			var endSession = new NewgroundsIO.components.App.endSession();
			endSession.SetCore(__ngioCore);
			yield return __ngioCore.ExecuteComponent(endSession, this.OnEndSession);
		}

		// Handler for EndSession. Resets the session locally
		protected void OnEndSession(NewgroundsIO.objects.Response response)
		{
			// We'll just clear out the whole session, even if something failed.
			this.ResetSession();
			this.id = null;
			this.mode = "new";
			this.status = NewgroundsIO.SessionState.USER_LOGGED_OUT;

			// Let our update loop know it can actually do stuff again
			this._canUpdate = true;
		}
	}

}

