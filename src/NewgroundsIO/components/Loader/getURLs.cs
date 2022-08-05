using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Loader {

	public class getURLs : NewgroundsIO.BaseComponent {


		public getURLs()
		{
			this.__object = "Loader.getURLs";

		}

		public NewgroundsIO.components.Loader.getURLs clone() {
			var clone = new NewgroundsIO.components.Loader.getURLs();
			clone.__properties.ForEach(propName => {
				clone.GetType().GetProperty(propName).SetValue(clone, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			clone.__ngioCore = this.__ngioCore;
			return clone;
		}

	}

}

