using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.CloudSave {

	public class loadSlots : NewgroundsIO.BaseResult {

		/// <summary>An array of #SaveSlot objects.</summary>
		public List<NewgroundsIO.objects.SaveSlot> slots { get; set; } = new List<NewgroundsIO.objects.SaveSlot>();


		/// <summary>Constructor</summary>
		public loadSlots()
		{
			this.__object = "CloudSave.loadSlots";

			this.__properties.Add("slots");
			this.__objectMap.Add("slots","SaveSlot");
		}

		/// <summary>Adds objects to their associated lists and casts them to their appropriate class.</summary>
		public override void AddToPropertyList( string propName, NewgroundsIO.BaseObject obj)
		{
			switch(propName) {

				case "slots":

					this.slots.Add(obj as NewgroundsIO.objects.SaveSlot);
					break;

			}
		}
		/// <summary>Links a Core instance to every object in our object lists.</summary>
		public override void SetCoreOnLists( NewgroundsIO.Core ngio )
		{
			this.slots.ForEach(child => { if (!(child is null)) child.SetCore(ngio); });
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.CloudSave.loadSlots clone(NewgroundsIO.results.CloudSave.loadSlots cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.CloudSave.loadSlots();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

