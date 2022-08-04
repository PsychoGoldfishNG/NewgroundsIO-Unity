using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.CloudSave {

	public class loadSlot : NewgroundsIO.BaseResult {

		/// <summary>A #SaveSlot object.</summary>
		public NewgroundsIO.objects.SaveSlot slot { get; set; }


		/// <summary>Constructor</summary>
		public loadSlot()
		{
			this.__object = "CloudSave.loadSlot";

			this.__properties.Add("slot");
			this.__objectMap.Add("slot","SaveSlot");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.CloudSave.loadSlot clone(NewgroundsIO.results.CloudSave.loadSlot cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.CloudSave.loadSlot();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

