using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewgroundsIO.objects {

	public class Error : NewgroundsIO.BaseObject {

		/// <summary>Contains details about the error.</summary>
		public string message { get; set; }

		/// <summary>A code indication the error type.</summary>
		public int code { get; set; }


		/// <summary>Constructor</summary>
		public Error()
		{
			this.__object = "Error";

			this.__properties.Add("message");
			this.__properties.Add("code");
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.Error clone(NewgroundsIO.objects.Error cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.Error();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

	}

}

