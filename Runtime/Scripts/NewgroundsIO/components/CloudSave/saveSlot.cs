using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.CloudSave {

	public class saveSlot : NewgroundsIO.BaseComponent {

		public int id { get; set; }
		public string data { get; set; }

		public saveSlot()
		{
			this.__object = "CloudSave.saveSlot";

			this.__properties.Add("id");
			this.__properties.Add("data");
			this.__required.Add("id");
			this.__required.Add("data");
			this.__requireSession = true;
		}

		public NewgroundsIO.components.CloudSave.saveSlot clone(NewgroundsIO.components.CloudSave.saveSlot cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.CloudSave.saveSlot();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

