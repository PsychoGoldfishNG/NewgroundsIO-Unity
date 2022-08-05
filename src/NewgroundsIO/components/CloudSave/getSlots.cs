using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.CloudSave {

	public class getSlots : NewgroundsIO.BaseComponent {


		public getSlots()
		{
			this.__object = "CloudSave.getSlots";

		}

		public NewgroundsIO.components.CloudSave.getSlots clone() {
			var clone = new NewgroundsIO.components.CloudSave.getSlots();
			clone.__properties.ForEach(propName => {
				clone.GetType().GetProperty(propName).SetValue(clone, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			clone.__ngioCore = this.__ngioCore;
			return clone;
		}

	}

}

