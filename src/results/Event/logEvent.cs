using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Event {

	public class logEvent : NewgroundsIO.BaseResult {

		public string event_name { get; set; }


		/// <summary>Constructor</summary>
		public logEvent()
		{
			this.__object = "Event.logEvent";

			this.__properties.Add("event_name");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Event.logEvent clone(NewgroundsIO.results.Event.logEvent cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Event.logEvent();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

