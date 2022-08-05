using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIO.objects {

	/// <summary>Contains all return output from an API request.</summary>
	public class Response : NewgroundsIO.BaseObject {

		/// <summary>Your application's unique ID</summary>
		public string app_id { get; set; }

		/// <summary>If false, there was a problem with your 'request' object. Details will be in the error property.</summary>
		public bool success { get; set; }

		/// <summary>Contains extra information you may need when debugging (debug mode only).</summary>
		public NewgroundsIO.objects.Debug debug { get; set; }

		/// <summary>This will contain any error info if the success property is false.</summary>
		public NewgroundsIO.objects.Error error { get; set; }

		/// <summary>If there was an error, this will contain the current version number of the API gateway.</summary>
		public string api_version { get; set; }

		/// <summary>If there was an error, this will contain the URL for our help docs.</summary>
		public string help_url { get; set; }

		/// <summary>If you passed an 'echo' value in your request object, it will be echoed here.</summary>
		public object echo { get; set; }


		/// <summary>If this is a list of queued responses, this will be true.</summary>
		public bool isList { get; private set; }

		/// <summary>Single results will be stored here.</summary>
		private NewgroundsIO.BaseResult _result = null;

		/// <summary>Queued results will be stored here</summary>
		private List<NewgroundsIO.BaseResult> _resultList = null;

		/// <summary>Constructor</summary>
		public Response()
		{
			this.__object = "Response";

			this.__properties.Add("app_id");
			this.__properties.Add("success");
			this.__properties.Add("debug");
			this.__properties.Add("error");
			this.__properties.Add("api_version");
			this.__properties.Add("help_url");
			this.__properties.Add("echo");
			this.__objectMap.Add("debug","Debug");
			this.__objectMap.Add("error","Error");
			this.__properties.Add("result");
			this.__properties.Add("resultList");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Response clone(NewgroundsIO.objects.Response cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Response();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

		/// <summary>A single component result.<summary>
		public NewgroundsIO.BaseResult result {
			get {
				return this._result;
			}
		}

		/// <summary>A list of component results.<summary>
		public List<NewgroundsIO.BaseResult> resultList {
			get {
				return this._resultList;
			}
		}

		/// <summary>Set a single results object from deserialized JSON.<summary>
		public void SetResults(JObject jObj)
		{
			string _component = (string)jObj.GetValue("component").ToObject(typeof(string));
			this._result = NewgroundsIO.ObjectIndex.CreateResult(_component, jObj.GetValue("data") as JObject);

			this.isList = false;
		}

		/// <summary>Set a list of results object from deserialized JSON.<summary>
		public void SetResultsList(JArray jArr)
		{
			string _component;
			this._resultList = new List<NewgroundsIO.BaseResult>();
			foreach (JObject jObj in jArr) {
				_component = (string)jObj.GetValue("component").ToObject(typeof(string));
				this._resultList.Add( NewgroundsIO.ObjectIndex.CreateResult(_component, jObj.GetValue("data") as JObject) );
			}

			this.isList = true;
		}
		/// <summary>This override will link a Core instance to every result in the resultList.</summary>
		public override void SetCoreOnLists( NewgroundsIO.Core ngio )
		{
			if (!(this._resultList is null)) this._resultList.ForEach(child => { if (!(child is null)) child.SetCore(ngio); });
		}

	}

}

