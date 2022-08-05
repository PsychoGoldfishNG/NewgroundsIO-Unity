using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.CloudSave {

	/// <summary>Saves data to a save slot. Any existing data will be replaced.</summary>
	public class setData : NewgroundsIO.BaseComponent {

		/// <summary>The slot number.</summary>
		public int id { get; set; }

		/// <summary>The data you want to save.</summary>
		public string data { get; set; }


		/// <summary>Constructor</summary>
		public setData()
		{
			this.__object = "CloudSave.setData";

			this.__properties.Add("id");
			this.__properties.Add("data");
			this.__required.Add("id");
			this.__required.Add("data");
			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.CloudSave.setData clone(NewgroundsIO.components.CloudSave.setData cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.CloudSave.setData();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

