using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.ScoreBoard {

	/// <summary>Posts a score to the specified scoreboard.</summary>
	public class postScore : NewgroundsIO.BaseComponent {

		/// <summary>The numeric ID of the scoreboard.</summary>
		public int id { get; set; }

		/// <summary>The int value of the score.</summary>
		public int value { get; set; }

		/// <summary>An optional tag that can be used to filter scores via ScoreBoard.getScores</summary>
		public string tag { get; set; }


		/// <summary>Constructor</summary>
		public postScore()
		{
			this.__object = "ScoreBoard.postScore";

			this.__properties.Add("id");
			this.__properties.Add("value");
			this.__properties.Add("tag");
			this.__required.Add("id");
			this.__required.Add("value");
			this.__isSecure = true;
			this.__requireSession = true;
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.ScoreBoard.postScore clone(NewgroundsIO.components.ScoreBoard.postScore cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.ScoreBoard.postScore();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

