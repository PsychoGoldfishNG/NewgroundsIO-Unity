using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Loader {

	public class loadOfficialUrl : NewgroundsIO.BaseResult {

		public string url { get; set; }


		/// <summary>Constructor</summary>
		public loadOfficialUrl()
		{
			this.__object = "Loader.loadOfficialUrl";

			this.__properties.Add("url");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Loader.loadOfficialUrl clone(NewgroundsIO.results.Loader.loadOfficialUrl cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Loader.loadOfficialUrl();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

