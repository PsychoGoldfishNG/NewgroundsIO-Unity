using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.components.Event {

	/// <summary>Logs a custom event to your API stats.</summary>
	public class logEvent : NewgroundsIO.BaseComponent {

		/// <summary>The name of your custom event as defined in your Referrals & Events settings.</summary>
		public string event_name { get; set; }


		/// <summary>Constructor</summary>
		public logEvent()
		{
			this.__object = "Event.logEvent";

			this.__properties.Add("host");
			this.__properties.Add("event_name");
			this.__required.Add("event_name");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.components.Event.logEvent clone(NewgroundsIO.components.Event.logEvent cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.components.Event.logEvent();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

