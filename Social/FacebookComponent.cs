using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Festi;
using Festi.Event;
using Festi.Social;
using Game;
using Facebook;
using Facebook.MiniJSON;

public class FacebookComponent : Festi.Social.SocialComponent 
{
	public override void OnInit()
	{	
	} // end OnInit

	public override void OnStart()
	{
		FB.Init(OnApiInitComplete);
	} // end OnStart

	protected void OnApiInitComplete()
	{
		this._FireEvent(SocialComponent.ON_API_INIT_COMPLETE);
	
		if (!this.IsLogin()) {
			this.Login();
		}
	} // end OnApiInitComplete

	public Boolean IsLogin()
	{
		return FB.IsLoggedIn;
	} // end IsLogin

	public override void Login(string scope = "email,publish_actions")
	{
		FB.Login(scope, OnApiLogin);
	} // end Login
	
	protected void OnApiLogin(FBResult result)
	{
		string eventType =  SocialComponent.ON_API_LOGIN_ERROR;

		if (this.IsLogin()) {
			eventType =  SocialComponent.ON_API_LOGIN_SUCCESS;
		}

		this._FireEvent(eventType);
	} // end OnApiLogin

	public override void LoadAppFriends(string fields = "id,name", int limit = 100)
	{
		string query = "/me?fields=" + fields + ",friends.limit(" + limit.ToString() + ").fields(" + fields + ")";
		FB.API(query, Facebook.HttpMethod.GET, OnLoadLoadAppFriends);
	} // end LoadAppFriends

	protected void OnLoadLoadAppFriends(FBResult result)
	{
		if (result.Error != null) {
			this._FireLoadError("LoadAppFriends");
			return;
		}
		
		List<object> friendsList = (List<object>) Util.DeserializeJSONFriends(result.Text);

		Dictionary<string, Friend> friends = new Dictionary<string, Friend>();
		foreach (Dictionary<string, object> friendData in friendsList) {
			Friend friend = new Friend(friendData, this);
			string id = friend.GetID();
			friends.Add(id, friend);
		}

		GameEvent apiEvent = new GameEvent(friends);
		this.Dispatch(FacebookComponent.ON_LOAD_APP_FRIENDS_COMPLETE, apiEvent);
	} // end OnLoadLoadAppFriends

	public override void LoadAvatar(SocialUser user, CallbackDelegate callback, int? width = null, int? height = null)
	{
		string url = Util.GetPictureURL(user.GetID(), width, height);

		FB.API(url, Facebook.HttpMethod.GET, delegate(FBResult result) { 

			if (result.Error != null) {
				callback(null);
				return;
			}
			
			var imageUrl = Util.DeserializePictureURLString(result.Text);

			StartCoroutine(_LoadPictureEnumerator(imageUrl, callback));
		});
	} // end LoadAvatar

	private IEnumerator _LoadPictureEnumerator(string url, CallbackDelegate callback)    
	{
		WWW www = new WWW(url);
		yield return www;
		callback(www.texture);
	} // end _LoadPictureEnumerator

	private void _FireLoadError(string methodName)
	{
		GameEvent apiEvent = new GameEvent(methodName);
		this.Dispatch(FacebookComponent.ON_METHOD_LOAD_ERROR, apiEvent);
	} // end _FireLoadError

	private void _FireEvent(string eventType)
	{
		GameEvent apiEvent = new GameEvent(this);
		this.Dispatch(eventType, apiEvent);
	} // end _FireEvent

	public override void SendInviteRequest(string invateMessage = "")                                                                                              
	{ 
		FB.AppRequest(
			message: invateMessage, 
			callback: InviteRequestCallback
		);
	} // end SendInviteRequest
	
	protected void InviteRequestCallback(FBResult result)                                                                              
	{   
		Util.Log("InviteRequestCallback");                                                                                         
		if (result == null) { 
			return;
		}

		var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;                                      
		object obj = 0;                                                                                                        
		if (responseObject.TryGetValue ("cancelled", out obj)) {                                                                                                                      
			Util.Log("Request cancelled");                                                                                  
		} else if (responseObject.TryGetValue ("request", out obj)) {                
			Util.Log("Request sent");                                                                                       
		}                                                                                                                      
	} // end InviteRequestCallback
}