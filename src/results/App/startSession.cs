using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.App {

	public class startSession : NewgroundsIO.BaseResult {

		public NewgroundsIO.objects.Session session { get; set; }


		/// <summary>Constructor</summary>
		public startSession()
		{
			this.__object = "App.startSession";

			this.__properties.Add("session");
			this.__objectMap.Add("session","Session");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.App.startSession clone(NewgroundsIO.results.App.startSession cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.App.startSession();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

