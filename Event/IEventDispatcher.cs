using UnityEngine;
using System.Collections;

namespace Festi.Event
{

	public interface IEventDispatcher
	{
		void AddListener (string type, EventCallback newMessageHandler);
		void RemoveListener (string type, EventCallback messageHandler);
		void Dispatch (string type, GameEvent message);
	}
	
	public delegate void EventCallback(GameEvent someEvent);
		
}