using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains extra debugging information.</summary>
	public class Debug : NewgroundsIO.BaseObject {

		/// <summary>The time, in milliseconds, that it took to execute a request.</summary>
		public string exec_time { get; set; }

		/// <summary>A copy of the request object that was posted to the server.</summary>
		public NewgroundsIO.objects.Request request { get; set; }


		/// <summary>Constructor</summary>
		public Debug()
		{
			this.__object = "Debug";

			this.__properties.Add("exec_time");
			this.__properties.Add("request");
			this.__objectMap.Add("request","Request");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Debug clone(NewgroundsIO.objects.Debug cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Debug();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

