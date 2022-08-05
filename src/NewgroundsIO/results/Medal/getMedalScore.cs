using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Medal {

	public class getMedalScore : NewgroundsIO.BaseResult {

		/// <summary>The user's medal score.</summary>
		public int medal_score { get; set; }


		/// <summary>Constructor</summary>
		public getMedalScore()
		{
			this.__object = "Medal.getMedalScore";

			this.__properties.Add("medal_score");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Medal.getMedalScore clone(NewgroundsIO.results.Medal.getMedalScore cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Medal.getMedalScore();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

