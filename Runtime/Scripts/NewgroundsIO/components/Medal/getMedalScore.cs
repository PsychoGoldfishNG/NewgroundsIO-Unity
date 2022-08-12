using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Medal {

	/// <summary>Loads the user's current medal score.</summary>
	public class getMedalScore : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public getMedalScore()
		{
			this.__object = "Medal.getMedalScore";

			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Medal.getMedalScore clone(NewgroundsIO.components.Medal.getMedalScore cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Medal.getMedalScore();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

