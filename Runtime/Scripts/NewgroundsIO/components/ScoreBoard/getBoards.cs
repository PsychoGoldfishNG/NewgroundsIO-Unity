using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.ScoreBoard {

	/// <summary>Returns a list of available scoreboards.</summary>
	public class getBoards : NewgroundsIO.BaseComponent {


		/// <summary>Constructor</summary>
		public getBoards()
		{
			this.__object = "ScoreBoard.getBoards";

		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.ScoreBoard.getBoards clone(NewgroundsIO.components.ScoreBoard.getBoards cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.ScoreBoard.getBoards();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

