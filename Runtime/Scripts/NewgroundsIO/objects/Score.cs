using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains information about a score posted to a scoreboard.</summary>
	public class Score : NewgroundsIO.BaseObject {

		/// <summary>The user who earned score. If this property is absent, the score belongs to the active user.</summary>
		public NewgroundsIO.objects.User user { get; set; }

		/// <summary>The integer value of the score.</summary>
		public int value { get; set; }

		/// <summary>The score value in the format selected in your scoreboard settings.</summary>
		public string formatted_value { get; set; }

		/// <summary>The tag attached to this score (if any).</summary>
		public string tag { get; set; }


		/// <summary>Constructor</summary>
		public Score()
		{
			this.__object = "Score";

			this.__properties.Add("user");
			this.__properties.Add("value");
			this.__properties.Add("formatted_value");
			this.__properties.Add("tag");
			this.__objectMap.Add("user","User");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Score clone(NewgroundsIO.objects.Score cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Score();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

