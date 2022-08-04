using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.App {

	public class getHostLicense : NewgroundsIO.BaseResult {

		public bool host_approved { get; set; }


		/// <summary>Constructor</summary>
		public getHostLicense()
		{
			this.__object = "App.getHostLicense";

			this.__properties.Add("host_approved");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.App.getHostLicense clone(NewgroundsIO.results.App.getHostLicense cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.App.getHostLicense();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

