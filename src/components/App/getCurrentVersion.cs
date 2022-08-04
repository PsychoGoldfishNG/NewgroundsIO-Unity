using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.App {

	/// <summary>Gets the version number of the app as defined in your "Version Control" settings.</summary>
	public class getCurrentVersion : NewgroundsIO.BaseComponent {

		/// <summary>The version number (in "X.Y.Z" format) of the client-side app. (default = "0.0.0")</summary>
		public string version { get; set; } = "0.0.0";


		/// <summary>Constructor</summary>
		public getCurrentVersion()
		{
			this.__object = "App.getCurrentVersion";

			this.__properties.Add("version");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.App.getCurrentVersion clone(NewgroundsIO.components.App.getCurrentVersion cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.App.getCurrentVersion();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

