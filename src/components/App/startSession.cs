using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.App {

	/// <summary>Starts a new session for the application.</summary>
	public class startSession : NewgroundsIO.BaseComponent {

		/// <summary>
		/// If true, will create a new session even if the user already has an existing one.
		/// 
		/// Note: Any previous session ids will no longer be valid if this is used.</summary>
		public bool force { get; set; }


		/// <summary>Constructor</summary>
		public startSession()
		{
			this.__object = "App.startSession";

			this.__properties.Add("force");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.App.startSession clone(NewgroundsIO.components.App.startSession cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.App.startSession();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

