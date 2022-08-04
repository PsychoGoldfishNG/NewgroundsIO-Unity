using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.App {

	/// <summary>Checks the validity of a session id and returns the results in a #Session object.</summary>
	public class checkSession : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public checkSession()
		{
			this.__object = "App.checkSession";

			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.App.checkSession clone(NewgroundsIO.components.App.checkSession cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.App.checkSession();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

