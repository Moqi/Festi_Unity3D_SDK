using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Festi.Event
{
	public class EventDispatcher : IEventDispatcher
	{
		private Dictionary<string, List<EventCallback>> _listeners = new Dictionary<string, List<EventCallback>>();
		
		public void AddListener (string type, EventCallback callback)
		{
			if (this._listeners.ContainsKey(type)) {	
				this._listeners[type].Add(callback);
			} else {
				List<EventCallback> newCallbackList = new List<EventCallback>();
				newCallbackList.Add(callback);
				this._listeners.Add(type, newCallbackList);
			}	
		}
		
		public void RemoveListener (string type, EventCallback callback)
		{
			if (this._listeners.ContainsKey(type)) {
				this._listeners[type].Remove(callback);
			}
		}
		
		public void Dispatch(string type, GameEvent evt)
		{
			if (!this._listeners.ContainsKey(type)) {
				return;
			}
			
			foreach (EventCallback callback in this._listeners[type]) {

				if (!evt.IsPropagated()) {
					continue;	
				}

				Delegate clb = (Delegate) callback;
				callback(evt);
			}	
		
		} // end Dispatch
		 
	}
}