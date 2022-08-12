using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.App {

	public class getCurrentVersion : NewgroundsIO.BaseResult {

		/// <summary>The version number of the app as defined in your "Version Control" settings.</summary>
		public string current_version { get; set; }

		/// <summary>Notes whether the client-side app is using a lower version number.</summary>
		public bool client_deprecated { get; set; }


		/// <summary>Constructor</summary>
		public getCurrentVersion()
		{
			this.__object = "App.getCurrentVersion";

			this.__properties.Add("current_version");
			this.__properties.Add("client_deprecated");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.App.getCurrentVersion clone(NewgroundsIO.results.App.getCurrentVersion cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.App.getCurrentVersion();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

