using UnityEngine;
using System.Collections;
using Festi.Event;

namespace Festi
{
	public class FestiMonoBehaviour : MonoBehaviour, IEventDispatcher
	{
		private EventDispatcher _eventDispatcher = new EventDispatcher();
		
		public void AddListener (string type, EventCallback evnt)
		{
			this._eventDispatcher.AddListener(type, evnt);
		}
		
		public void RemoveListener (string type, EventCallback evnt)
		{
			this._eventDispatcher.RemoveListener(type, evnt);
		}
		
		public void Dispatch (string type, GameEvent evnt)
		{
			this._eventDispatcher.Dispatch(type, evnt);
		}
	}
}