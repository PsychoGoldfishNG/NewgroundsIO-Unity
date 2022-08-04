using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Loader {

	/// <summary>
	/// Loads the official URL of the app's author (as defined in your "Official URLs" settings), and logs a referral to your API stats.
	/// 
	/// For apps with multiple author URLs, use Loader.loadReferral.</summary>
	public class loadAuthorUrl : NewgroundsIO.BaseComponent {

		/// <summary>Set this to false to get a JSON response containing the URL instead of doing an actual redirect.</summary>
		public bool redirect { get; set; } = true;

		/// <summary>Set this to false to skip logging this as a referral event.</summary>
		public bool log_stat { get; set; } = true;


		/// <summary>Constructor</summary>
		public loadAuthorUrl()
		{
			this.__object = "Loader.loadAuthorUrl";

			this.__properties.Add("host");
			this.__properties.Add("redirect");
			this.__properties.Add("log_stat");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Loader.loadAuthorUrl clone(NewgroundsIO.components.Loader.loadAuthorUrl cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Loader.loadAuthorUrl();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

