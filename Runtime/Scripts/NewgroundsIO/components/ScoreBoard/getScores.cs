using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.ScoreBoard {

	/// <summary>Loads a list of #Score objects from a scoreboard. Use 'skip' and 'limit' for getting different pages.</summary>
	public class getScores : NewgroundsIO.BaseComponent {

		/// <summary>The numeric ID of the scoreboard.</summary>
		public int id { get; set; }

		/// <summary>The time-frame to pull scores from (see notes for acceptable values).</summary>
		public string period { get; set; }

		/// <summary>A tag to filter results by.</summary>
		public string tag { get; set; }

		/// <summary>If set to true, only social scores will be loaded (scores by the user and their friends). This param will be ignored if there is no valid session id and the 'user' param is absent.</summary>
		public bool social { get; set; }

		/// <summary>A user's ID or name.  If 'social' is true, this user and their friends will be included. Otherwise, only scores for this user will be loaded. If this param is missing and there is a valid session id, that user will be used by default.</summary>
		public object user { get; set; }

		/// <summary>An integer indicating the number of scores to skip before starting the list. Default = 0.</summary>
		public int skip { get; set; } = 0;

		/// <summary>An integer indicating the number of scores to include in the list. Default = 10.</summary>
		public int limit { get; set; } = 10;


		/// <summary>Constructor</summary>
		public getScores()
		{
			this.__object = "ScoreBoard.getScores";

			this.__properties.Add("id");
			this.__properties.Add("period");
			this.__properties.Add("tag");
			this.__properties.Add("social");
			this.__properties.Add("user");
			this.__properties.Add("skip");
			this.__properties.Add("limit");
			this.__required.Add("id");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.ScoreBoard.getScores clone(NewgroundsIO.components.ScoreBoard.getScores cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.ScoreBoard.getScores();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

