using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains information about a user.</summary>
	public class User : NewgroundsIO.BaseObject {

		/// <summary>The user's numeric ID.</summary>
		public int id { get; set; }

		/// <summary>The user's textual name.</summary>
		public string name { get; set; }

		/// <summary>The user's icon images.</summary>
		public NewgroundsIO.objects.UserIcons icons { get; set; }

		/// <summary>Returns true if the user has a Newgrounds Supporter upgrade.</summary>
		public bool supporter { get; set; }


		/// <summary>Constructor</summary>
		public User()
		{
			this.__object = "User";

			this.__properties.Add("id");
			this.__properties.Add("name");
			this.__properties.Add("icons");
			this.__properties.Add("supporter");
			this.__objectMap.Add("icons","UserIcons");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.User clone(NewgroundsIO.objects.User cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.User();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

