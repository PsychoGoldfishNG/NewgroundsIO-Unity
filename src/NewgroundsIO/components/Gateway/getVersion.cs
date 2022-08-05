using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Gateway {

	/// <summary>Returns the current version of the Newgrounds.io gateway.</summary>
	public class getVersion : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public getVersion()
		{
			this.__object = "Gateway.getVersion";

		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Gateway.getVersion clone(NewgroundsIO.components.Gateway.getVersion cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Gateway.getVersion();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

