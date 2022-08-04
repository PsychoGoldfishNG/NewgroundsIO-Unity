using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.results.Gateway {

	public class ping : NewgroundsIO.BaseResult {

		/// <summary>Will always return a value of 'pong'</summary>
		public string pong { get; set; }


		/// <summary>Constructor</summary>
		public ping()
		{
			this.__object = "Gateway.ping";

			this.__properties.Add("pong");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.results.Gateway.ping clone(NewgroundsIO.results.Gateway.ping cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.results.Gateway.ping();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

