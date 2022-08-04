using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.ScoreBoard {

	public class postScore : NewgroundsIO.BaseResult {

		/// <summary>The #ScoreBoard that was posted to.</summary>
		public NewgroundsIO.objects.ScoreBoard scoreboard { get; set; }

		/// <summary>The #Score that was posted to the board.</summary>
		public NewgroundsIO.objects.Score score { get; set; }


		/// <summary>Constructor</summary>
		public postScore()
		{
			this.__object = "ScoreBoard.postScore";

			this.__properties.Add("scoreboard");
			this.__properties.Add("score");
			this.__objectMap.Add("scoreboard","ScoreBoard");
			this.__objectMap.Add("score","Score");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.ScoreBoard.postScore clone(NewgroundsIO.results.ScoreBoard.postScore cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.ScoreBoard.postScore();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

