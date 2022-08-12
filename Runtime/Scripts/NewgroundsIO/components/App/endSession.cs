using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.App {

	/// <summary>Ends the current session, if any.</summary>
	public class endSession : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public endSession()
		{
			this.__object = "App.endSession";

			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.App.endSession clone(NewgroundsIO.components.App.endSession cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.App.endSession();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

