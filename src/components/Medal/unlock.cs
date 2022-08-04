using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Medal {

	/// <summary>Unlocks a medal.</summary>
	public class unlock : NewgroundsIO.BaseComponent {

		/// <summary>The numeric ID of the medal to unlock.</summary>
		public int id { get; set; }


		/// <summary>Constructor</summary>
		public unlock()
		{
			this.__object = "Medal.unlock";

			this.__properties.Add("id");
			this.__required.Add("id");
			this.__isSecure = true;
			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Medal.unlock clone(NewgroundsIO.components.Medal.unlock cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Medal.unlock();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

