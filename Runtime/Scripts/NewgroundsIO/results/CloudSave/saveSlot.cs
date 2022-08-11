using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.CloudSave {

	public class saveSlot : NewgroundsIO.BaseResult {

		public NewgroundsIO.objects.SaveSlot slot { get; set; }

		public saveSlot()
		{
			this.__object = "CloudSave.saveSlot";

			this.__properties.Add("slot");
			this.__objectMap.Add("slot","SaveSlot");
		}

		public NewgroundsIO.results.CloudSave.saveSlot clone(NewgroundsIO.results.CloudSave.saveSlot cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.CloudSave.saveSlot();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

