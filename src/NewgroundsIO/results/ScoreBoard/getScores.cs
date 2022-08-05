using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.ScoreBoard {

	public class getScores : NewgroundsIO.BaseResult {

		/// <summary>The time-frame the scores belong to. See notes for acceptable values.</summary>
		public string period { get; set; }

		/// <summary>Will return true if scores were loaded in social context ('social' set to true and a session or 'user' were provided).</summary>
		public bool social { get; set; }

		/// <summary>The query skip that was used.</summary>
		public int limit { get; set; }

		/// <summary>The #ScoreBoard being queried.</summary>
		public NewgroundsIO.objects.ScoreBoard scoreboard { get; set; }

		/// <summary>An array of #Score objects.</summary>
		public List<NewgroundsIO.objects.Score> scores { get; set; } = new List<NewgroundsIO.objects.Score>();

		/// <summary>The #User the score list is associated with (either as defined in the 'user' param, or extracted from the current session when 'social' is set to true)</summary>
		public NewgroundsIO.objects.User user { get; set; }


		/// <summary>Constructor</summary>
		public getScores()
		{
			this.__object = "ScoreBoard.getScores";

			this.__properties.Add("period");
			this.__properties.Add("social");
			this.__properties.Add("limit");
			this.__properties.Add("scoreboard");
			this.__properties.Add("scores");
			this.__properties.Add("user");
			this.__objectMap.Add("scoreboard","ScoreBoard");
			this.__objectMap.Add("scores","Score");
			this.__objectMap.Add("user","User");
		}

		/// <summary>Adds objects to their associated lists and casts them to their appropriate class.</summary>
		public override void AddToPropertyList( string propName, NewgroundsIO.BaseObject obj)
		{
			switch(propName) {

				case "scores":

					this.scores.Add(obj as NewgroundsIO.objects.Score);
					break;

			}
		}
		/// <summary>Links a Core instance to every object in our object lists.</summary>
		public override void SetCoreOnLists( NewgroundsIO.Core ngio )
		{
			this.scores.ForEach(child => { if (!(child is null)) child.SetCore(ngio); });
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.ScoreBoard.getScores clone(NewgroundsIO.results.ScoreBoard.getScores cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.ScoreBoard.getScores();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

