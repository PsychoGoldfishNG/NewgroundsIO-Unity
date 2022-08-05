using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Medal {

	public class getList : NewgroundsIO.BaseResult {

		/// <summary>An array of medal objects.</summary>
		public List<NewgroundsIO.objects.Medal> medals { get; set; } = new List<NewgroundsIO.objects.Medal>();


		/// <summary>Constructor</summary>
		public getList()
		{
			this.__object = "Medal.getList";

			this.__properties.Add("medals");
			this.__objectMap.Add("medals","Medal");
		}

		/// <summary>Adds objects to their associated lists and casts them to their appropriate class.</summary>
		public override void AddToPropertyList( string propName, NewgroundsIO.BaseObject obj)
		{
			switch(propName) {

				case "medals":

					this.medals.Add(obj as NewgroundsIO.objects.Medal);
					break;

			}
		}
		/// <summary>Links a Core instance to every object in our object lists.</summary>
		public override void SetCoreOnLists( NewgroundsIO.Core ngio )
		{
			this.medals.ForEach(child => { if (!(child is null)) child.SetCore(ngio); });
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Medal.getList clone(NewgroundsIO.results.Medal.getList cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Medal.getList();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

