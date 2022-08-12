using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains any icons associated with this user.</summary>
	public class UserIcons : NewgroundsIO.BaseObject {

		/// <summary>The URL of the user's small icon</summary>
		public string small { get; set; }

		/// <summary>The URL of the user's medium icon</summary>
		public string medium { get; set; }

		/// <summary>The URL of the user's large icon</summary>
		public string large { get; set; }


		/// <summary>Constructor</summary>
		public UserIcons()
		{
			this.__object = "UserIcons";

			this.__properties.Add("small");
			this.__properties.Add("medium");
			this.__properties.Add("large");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.UserIcons clone(NewgroundsIO.objects.UserIcons cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.UserIcons();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

