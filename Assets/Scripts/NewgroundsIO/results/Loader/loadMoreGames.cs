using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Loader {

	public class loadMoreGames : NewgroundsIO.BaseResult {

		public string url { get; set; }


		/// <summary>Constructor</summary>
		public loadMoreGames()
		{
			this.__object = "Loader.loadMoreGames";

			this.__properties.Add("url");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Loader.loadMoreGames clone(NewgroundsIO.results.Loader.loadMoreGames cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Loader.loadMoreGames();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

