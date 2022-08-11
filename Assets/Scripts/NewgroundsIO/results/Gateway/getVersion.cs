using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Gateway {

	public class getVersion : NewgroundsIO.BaseResult {

		/// <summary>The version number (in X.Y.Z format).</summary>
		public string version { get; set; }


		/// <summary>Constructor</summary>
		public getVersion()
		{
			this.__object = "Gateway.getVersion";

			this.__properties.Add("version");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Gateway.getVersion clone(NewgroundsIO.results.Gateway.getVersion cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Gateway.getVersion();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

