using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Medal {

	public class unlock : NewgroundsIO.BaseResult {

		/// <summary>The #Medal that was unlocked.</summary>
		public NewgroundsIO.objects.Medal medal { get; set; }

		/// <summary>The user's new medal score.</summary>
		public int medal_score { get; set; }


		/// <summary>Constructor</summary>
		public unlock()
		{
			this.__object = "Medal.unlock";

			this.__properties.Add("medal");
			this.__properties.Add("medal_score");
			this.__objectMap.Add("medal","Medal");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Medal.unlock clone(NewgroundsIO.results.Medal.unlock cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Medal.unlock();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

