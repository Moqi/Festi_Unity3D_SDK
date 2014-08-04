using UnityEngine;
using System;
using System.Collections;
using Festi.Event;

namespace Festi.Network
{	
	public class ResponseEvent : GameEvent 
	{
		public const string SERVER_RESPONSE = "SERVER_RESPONCE";
		public const string RESPONSE_ERROR = "RESPONCE_ERROR";
		private WWW _response;
		private string _id;
		private string _responseString;
		
		public ResponseEvent (WWW wwwContent, IEventDispatcher target)
		{
			this.Responce = wwwContent;
			this.Target = (System.Object) target;	
		}
		
		public ResponseEvent (string responseString)
		{
			this._responseString = responseString;
		}
		
		private WWW Responce
		{
			get 
			{
				return this._response; 
			}
			set 
			{
				_response = value; 
			}
		}
		
		public Texture2D GetTexture()
		{
			if (IsErrorMesage()) {
				throw new System.Exception(" Download finished with error " + _response.error);
			}
			
			Texture2D texture = Responce.texture;
			
			if (texture == null) {
				throw new System.Exception("You are trying to get texture from server response, which can not be found in it.");
			}
			
			return texture;
		}
	
		public string GetString ()
		{
			if (!string.IsNullOrEmpty(this._responseString)) {
				return this._responseString;
			}
			
			if (Responce.assetBundle != null) { 
				throw new UnityException("Trying get string from downloaded content which contains AssetBundle.");
			}
			
			if (IsErrorMesage()) {
				throw new UnityException("Trying get string from content which finished with error");
			}
			
			string stringContent;
			
			try {
				stringContent = Responce.text;
			} catch (Exception e){ 
				throw new UnityException("Can't get text data from downloaded content ", e);
			}
			
			if (stringContent == null) {
				throw new Exception("Downloaded string is null ");
			}
			
			return stringContent;
		}
		
		public AssetBundle GetBundle ()
		{
			if (IsErrorMesage()) {
				throw new System.Exception(" AssetBundle responce finished with error " + Responce.error);
			}
			
			AssetBundle bundle = Responce.assetBundle;
			
			if (bundle == null) {
				throw new System.Exception("You are trying to get Asset bundle from server response, which can not be found in it.");
			}
			
			return bundle;
		}
			
		public AudioClip GetAudio ()
		{
			if (IsErrorMesage()) {
				throw new System.Exception(" Audio responce finished with error " + Responce.error);
			}
			AudioClip audioClip = null;
			try { // FIXME : this Try-catch block is here utill music on server will not be changed from OGG to MP3 or WAV for mobile
				audioClip =  this.Responce.audioClip;
				//audioClip = (AudioClip) Resources.Load("player_shooting 1", typeof(AudioClip));
			} catch {
				Debug.Log("ResponseEvent.GetAudio Error");
			}
			if (audioClip == null) {
				throw new System.Exception("You are trying to get audio clip from server response, which can not be found in it.");
			}
			
			return audioClip;
		}
		
		
		private bool IsErrorMesage ()
		{
			return Responce.error != null;
		}
		
		public void SetId (string id) 
		{
			this._id = id;
		}
		public string GetId ()	
		{
			return _id;
		}
	
	}
		
}
