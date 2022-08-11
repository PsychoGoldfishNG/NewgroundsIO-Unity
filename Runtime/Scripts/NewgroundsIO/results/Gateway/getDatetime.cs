using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Gateway {

	public class getDatetime : NewgroundsIO.BaseResult {

		/// <summary>The server's date and time in ISO 8601 format.</summary>
		public string datetime { get; set; }

		/// <summary>The current UNIX timestamp on the server.</summary>
		public int timestamp { get; set; }


		/// <summary>Constructor</summary>
		public getDatetime()
		{
			this.__object = "Gateway.getDatetime";

			this.__properties.Add("datetime");
			this.__properties.Add("timestamp");
		}

		/// <summary>Returns the datetime value as an actual DateTime</summary>
		public DateTime GetDateTime()
		{
			return DateTime.Parse(datetime);
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Gateway.getDatetime clone(NewgroundsIO.results.Gateway.getDatetime cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Gateway.getDatetime();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

