using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>A top-level wrapper containing any information needed to authenticate the application/user and any component calls being made.</summary>
	public class Request : NewgroundsIO.BaseObject {

		/// <summary>Your application's unique ID.</summary>
		public string app_id { get; set; }

		/// <summary>An optional login session id.</summary>
		public string session_id { get; set; }

		/// <summary>If set to true, calls will be executed in debug mode.</summary>
		public bool debug { get; set; }

		/// <summary>An optional value that will be returned, verbatim, in the #Response object.</summary>
		public object echo { get; set; }

		/// <summary>If this is a list of queued components, this will be true.</summary>
		public bool isList { get; set; } = false;

		/// <summary>An ExecuteWrapper object to run in this request.</summary>
		public NewgroundsIO.ExecuteWrapper execute { get; set; }

		/// <summary>A list of ExecuteWrapper objects to run in this request (if executing a queue).</summary>
		public List<NewgroundsIO.ExecuteWrapper> executeList { get; set; } = new List<NewgroundsIO.ExecuteWrapper>();

		/// <summary></summary>
		private bool __requireSession = false;

		/// <summary>Constructor</summary>
		public Request()
		{
			this.__object = "Request";

			this.__properties.Add("app_id");
			this.__properties.Add("session_id");
			this.__properties.Add("debug");
			this.__properties.Add("echo");
			this.__required.Add("app_id");
			// Add the execute property and make it required.
			this.__properties.Add("execute");
			this.__required.Add("execute");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Request clone(NewgroundsIO.objects.Request cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Request();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

		/// <summary></summary>
		/// <param name="propName">The property to encode.</param>
		public override string _getPropertyJSON(string propName) 
		{
			// This could be a single execute, or a whole queue. Decide which to encode.
			if (propName == "execute") {
				return "\"execute\":" + this._getValueJSON((this.isList ? this.executeList : this.execute));
			}

			return base._getPropertyJSON(propName);
		}

		/// <summary>Checks to see if we can skip including a property. In this case, debug.</summary>
		/// <param name="propName">The property to check.</param>
		public override bool _skipJsonProp(string propName)
		{ 
			if (propName == "debug" && !this.debug) return true;
			return base._skipJsonProp(propName);
		}

		/// <summary>Tells this request if a valid user session is required to execute one or more components.</summary>
		/// <param name="require">Set to true if a component requires a session.</param>
		public void RequiresSession(bool require)
		{ 
			__requireSession = require;

			// if we don't have a Core or session, this is a bad request
			if (require && ((__ngioCore is null) || (this.session_id is null))) {
				UnityEngine.Debug.LogError("NewgroundsIO Error: One or more components requires an active user session!");
			}

		}

		/// <summary>Links a Core to this request and extracts it's app ID.</summary>
		/// <param name="ngio">The Core instance.</param>
		public override void SetCore(Core ngio)
		{ 
			base.SetCore(ngio);
			this.app_id = ngio.appID;
			if (!(ngio.session?.id is null)) this.session_id = ngio.session.id;
		}

	}

}

