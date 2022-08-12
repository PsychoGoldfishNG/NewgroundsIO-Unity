using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Gateway {

	/// <summary>Loads the current date and time from the Newgrounds.io server.</summary>
	public class getDatetime : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public getDatetime()
		{
			this.__object = "Gateway.getDatetime";

		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Gateway.getDatetime clone(NewgroundsIO.components.Gateway.getDatetime cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Gateway.getDatetime();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

