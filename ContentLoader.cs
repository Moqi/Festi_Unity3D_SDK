using UnityEngine;
using System.Collections.Generic;
using Festi.Network;
using Festi.Event;

namespace Festi 
{
	public class ContentLoader : SceneComponent
	{
		private WWW _loader;
		private string _id;
		private float _timeOut = 10;
		private float _loadingTime = 0;
		private int _redownloadingTtl = 3;
		
		public void DownloadContent (string path) 
		{
			if (path.Substring(0, 7) == "http://") { // If server oriented command
				this._loader = new WWW(path);

			} else { // Have to find file in local resources
				
				TextAsset responceFile = (TextAsset) Resources.Load(path, typeof(TextAsset)) as TextAsset;
				Debug.Log("file found  ---->>>> " + responceFile.text );
				if (responceFile != null) {
					ResponseEvent responseEvent = new ResponseEvent(responceFile.text);
					Dispatch(ResponseEvent.SERVER_RESPONSE, responseEvent);
					GameObject.Destroy(this.gameObject);
				} else {
					Debug.Log("Response file == null!");
				}
			}
		}
		
		public void DownloadContent (string path, string id) 
		{	
			if (path.Substring(0, 7) == "http://") { // If server oriented command
				Debug.Log("Downloading content from : --- > " + path);
				this._loader = new WWW(path);
				this._id = id;

			} else { // Have to find file in local resources
				Debug.Log("Looking in local for  : --- > " + path);
				TextAsset responceFile = (TextAsset) Resources.Load(path, typeof(TextAsset)) as TextAsset;
				if (responceFile != null) {
					ResponseEvent responseEvent = new ResponseEvent(responceFile.text);
					Dispatch(ResponseEvent.SERVER_RESPONSE, responseEvent);
					GameObject.Destroy(this.gameObject);
				}
			}
		}
		
		private void Update()
		{
			if (this._loader == null) {
				return;
			}
			
			if (this._loader != null && this._loader.isDone) {
				OnDownloadComplete();
			}
			CheckTimeOut();
		}
		
		private void CheckTimeOut ()
		{
			_loadingTime += Time.deltaTime;
			
			if (_loadingTime > _timeOut) {
				RestartLoading();
			}
		}
		
		private void RestartLoading ()
		{
			if (_redownloadingTtl <= 0) {
				OnDownloadFailedByTtl();
				return;
			}
			_redownloadingTtl--;
			
//			Debug.Log("Loading timeout -- Restarting");
			_loadingTime = 0;
			string url = this._loader.url;
			_loader = new WWW(url);
		}
		
		private void OnDownloadComplete ()
		{
			if (this._loader.error != null) {
//				Debug.Log("Download finished with error " + _loader.error);
				Festi.Event.ErrorEvent errorEvent = new Festi.Event.ErrorEvent();
				errorEvent.SetText("Error unknown. " +  _loader.error);
				SceneController sceneController = SceneController.GetInstance();
				if (sceneController != null) {
					sceneController.Dispatch(Festi.Event.ErrorEvent.ERROR, errorEvent);
					this.Dispatch(Festi.Event.ErrorEvent.ERROR, errorEvent);
				} else {
					Debug.Log("Download finished with error");
					Debug.LogWarning("ContentLoader can not find controller on scene. Check if scene contains conroller class derived from Tantrum.SceneController");
				}
				RestartLoading();
				return;
			} 
			
			ResponseEvent responseEvent = new ResponseEvent(this._loader, this);
			if (!string.IsNullOrEmpty(this._id)) {
				responseEvent.SetId(this._id);
			}
			
			Dispatch(ResponseEvent.SERVER_RESPONSE, responseEvent);
			GameObject.Destroy(this.gameObject);
		}
						
		private void OnDownloadFailedByTtl ()
		{
//			Debug.Log("Download failed");
			
			ResponseEvent responseEvent = new ResponseEvent(this._loader, this);
			if (!string.IsNullOrEmpty(this._id)) {
				responseEvent.SetId(this._id);
			}
			
			Dispatch(ResponseEvent.RESPONSE_ERROR, responseEvent);
			
			Festi.Event.ErrorEvent errorEvent = new Festi.Event.ErrorEvent();
			errorEvent.SetText("Error. Content downloading failed.");
			SceneController.GetInstance().Dispatch(Festi.Event.ErrorEvent.ERROR, errorEvent);
				
			GameObject.Destroy(this.gameObject);
		}

		
		public static void Download (string url, EventCallback callback)
		{			
			GameObject newLoaderGameObject = new GameObject("ContentLoader");
			ContentLoader loader = newLoaderGameObject.AddComponent<ContentLoader>();
			loader.DownloadContent(url);
			loader.AddListener(ResponseEvent.SERVER_RESPONSE, callback);
		}
		
		public static void Download (string url, EventCallback callback, string id)
		{			
			GameObject newLoaderGameObject = new GameObject("ContentLoader");
			ContentLoader loader = newLoaderGameObject.AddComponent<ContentLoader>();
			loader.DownloadContent(url, id);
			loader.AddListener(ResponseEvent.SERVER_RESPONSE, callback);
		}
		
		public static void SendData (string url, string name, string data) 
		{
			GameObject newLoaderGameObject = new GameObject("ContentLoader");
			ContentLoader loader = newLoaderGameObject.AddComponent<ContentLoader>();
			loader.SendDataTo(url, name, data);
		}
		
		public void SendDataTo (string url, string name, string data)
		{
			WWWForm form = new WWWForm();
			
			form.AddField(name, data);
			
			this._loader = new WWW(url, form);
			
			Destroy(this.gameObject);
		}
				
	}
}
