using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.CloudSave {

	/// <summary>Deletes all data from a save slot.</summary>
	public class clearSlot : NewgroundsIO.BaseComponent {

		/// <summary>The slot number.</summary>
		public int id { get; set; }


		/// <summary>Constructor</summary>
		public clearSlot()
		{
			this.__object = "CloudSave.clearSlot";

			this.__properties.Add("id");
			this.__required.Add("id");
			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.CloudSave.clearSlot clone(NewgroundsIO.components.CloudSave.clearSlot cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.CloudSave.clearSlot();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

