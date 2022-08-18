using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NewgroundsIO.objects {

	/// <summary>Contains information about a CloudSave slot.</summary>
	public class SaveSlot : NewgroundsIO.BaseObject {

		/// <summary>The slot number.</summary>
		public int id { get; set; }

		/// <summary>The size of the save data in bytes.</summary>
		public int size { get; set; }

		/// <summary>A date and time (in ISO 8601 format) representing when this slot was last saved.</summary>
		public string datetime { get; set; }

		/// <summary>A unix timestamp representing when this slot was last saved.</summary>
		public int timestamp { get; set; }

		/// <summary>The URL containing the actual save data for this slot, or null if this slot has no data.</summary>
		public string url { get; set; }

		/// <summary>This will be true if this save slot has any saved data</summary>
		public bool hasData { get { return !(this.url is null); }}


		/// <summary>Constructor</summary>
		public SaveSlot()
		{
			this.__object = "SaveSlot";

			this.__properties.Add("id");
			this.__properties.Add("size");
			this.__properties.Add("datetime");
			this.__properties.Add("timestamp");
			this.__properties.Add("url");
		}

		/// <summary>Returns the datetime value as an actual DateTime</summary>
		public DateTime GetDateTime()
		{
			return DateTime.Parse(datetime);
		}

		/// <summary>Clones the properties of this object to another (or new) object.</summary>
		/// <param name="cloneTo">An object to clone properties to. If null, a new instance will be created.</param>
		/// <returns>The object that was cloned to.</returns>
		public NewgroundsIO.objects.SaveSlot clone(NewgroundsIO.objects.SaveSlot cloneTo = null) {
			if (cloneTo is null) cloneTo = new NewgroundsIO.objects.SaveSlot();
			cloneTo.__properties.ForEach(propName => {
				cloneTo.GetType().GetProperty(propName).SetValue(cloneTo, this.GetType().GetProperty(propName).GetValue(this), null);
			});
			cloneTo.__ngioCore = this.__ngioCore;
			return cloneTo;
		}

		/// <summary>Loads the save file for this slot then passes its contents to a callback function.</summary>
		/// <param name="callback">The callback function</param>
		public IEnumerator GetData(Action<string> callback)
		{
			if (this.url is null) {
				callback(null);
				yield break;
			}

		    UnityWebRequest www = UnityWebRequest.Get(this.url);
		    yield return www.SendWebRequest();

		    if (www.result != UnityWebRequest.Result.Success) {
		        callback(null);

		    } else {
		        callback(www.downloadHandler.text);
		    }
		}

		/// <summary>Saves string data to a file associated with this slot, and calls a function when complete.</summary>
		/// <param name="data">The data you want to save (needs to be serialized to a string).</param>
		/// <param name="callback">The callback function</param>
		public IEnumerator SetData(string data, Action<NewgroundsIO.objects.Response> callback=null)
		{
			if (__ngioCore is null) {
				if (!(callback is null)) callback(null);
				yield break;
			}

			var component = new NewgroundsIO.components.CloudSave.setData();
			component.id = this.id;
			component.data = data;
			yield return __ngioCore.ExecuteComponent(component, callback);

		}
	}

}

