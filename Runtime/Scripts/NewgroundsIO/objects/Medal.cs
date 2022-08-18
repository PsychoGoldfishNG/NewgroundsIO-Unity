using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	/// <summary>Contains information about a medal.</summary>
	public class Medal : NewgroundsIO.BaseObject {

		/// <summary>The numeric ID of the medal.</summary>
		public int id { get; set; }

		/// <summary>The name of the medal.</summary>
		public string name { get; set; }

		/// <summary>A short description of the medal.</summary>
		public string description { get; set; }

		/// <summary>The URL for the medal's icon.</summary>
		public string icon { get; set; }

		/// <summary>The medal's point value.</summary>
		public int value { get; set; }

		/// <summary>The difficulty id of the medal.</summary>
		public int difficulty { get; set; }

		public bool secret { get; set; }

		/// <summary>This will only be set if a valid user session exists.</summary>
		public bool unlocked { get; set; }


		/// <summary>Constructor</summary>
		public Medal()
		{
			this.__object = "Medal";

			this.__properties.Add("id");
			this.__properties.Add("name");
			this.__properties.Add("description");
			this.__properties.Add("icon");
			this.__properties.Add("value");
			this.__properties.Add("difficulty");
			this.__properties.Add("secret");
			this.__properties.Add("unlocked");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Medal clone(NewgroundsIO.objects.Medal cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Medal();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

		/// <summary>Unlocks this medal, then fires a callback.</summary>
		/// <param name="callback">An optional function to call when the medal is unlocked on the server.</summary>
		public IEnumerator Unlock(Action<NewgroundsIO.objects.Response> callback=null)
		{
			// You can't unlock a medal without a Core object.
			if (this.__ngioCore is null) {
				UnityEngine.Debug.LogError("NewgroundsIO - Can not unlock medal object without attaching a NewgroundsIO.Core instance.");
				yield break;
			}

			// Do the unlock
			var component = new NewgroundsIO.components.Medal.unlock();
			component.id = this.id;
			yield return __ngioCore.ExecuteComponent(component, callback);
		}

	}

}

