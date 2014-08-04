using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Festi.Social
{	
	public class SocialUser 
	{
		private SocialComponent _api;
		private string _ID;
		
		private Dictionary<string, object> _data;
		
		public SocialUser(Dictionary<string, object> data, SocialComponent api)
		{
			this._api = api;
			this._data = data;
			this._ID = (string) data["id"];
		} // end Friend
		
		public void LoadAvatar(SocialComponent.CallbackDelegate callback, int? width = null, int? height = null)
		{
			this._api.LoadAvatar(this, callback, width, height);
		}
		
		public string GetID()
		{
			return this._ID;
		} // end GetID
		
		public string Get(string key)
		{
			return (string) this._data[key];
		} // end Get
	} // end class SocialUser
}

