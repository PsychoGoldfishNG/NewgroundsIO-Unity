using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Medal {

	/// <summary>Loads a list of #Medal objects.</summary>
	public class getList : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public getList()
		{
			this.__object = "Medal.getList";

		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Medal.getList clone(NewgroundsIO.components.Medal.getList cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Medal.getList();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

