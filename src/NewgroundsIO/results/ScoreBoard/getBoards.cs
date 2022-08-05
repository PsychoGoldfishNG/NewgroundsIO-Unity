using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.ScoreBoard {

	public class getBoards : NewgroundsIO.BaseResult {

		/// <summary>An array of #ScoreBoard objects.</summary>
		public List<NewgroundsIO.objects.ScoreBoard> scoreboards { get; set; } = new List<NewgroundsIO.objects.ScoreBoard>();


		/// <summary>Constructor</summary>
		public getBoards()
		{
			this.__object = "ScoreBoard.getBoards";

			this.__properties.Add("scoreboards");
			this.__objectMap.Add("scoreboards","ScoreBoard");
		}

		/// <summary>Adds objects to their associated lists and casts them to their appropriate class.</summary>
		public override void AddToPropertyList( string propName, NewgroundsIO.BaseObject obj)
		{
			switch(propName) {

				case "scoreboards":

					this.scoreboards.Add(obj as NewgroundsIO.objects.ScoreBoard);
					break;

			}
		}
		/// <summary>Links a Core instance to every object in our object lists.</summary>
		public override void SetCoreOnLists( NewgroundsIO.Core ngio )
		{
			this.scoreboards.ForEach(child => { if (!(child is null)) child.SetCore(ngio); });
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.ScoreBoard.getBoards clone(NewgroundsIO.results.ScoreBoard.getBoards cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.ScoreBoard.getBoards();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

