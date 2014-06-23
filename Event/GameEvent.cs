using UnityEngine;
using System.Collections;

namespace Festi.Event
{

	public class GameEvent 
	{
		private bool _isPropagated = true;
		protected IEventDispatcher target;
		public IEventDispatcher Target
		{
			get 
			{
				return this.target; 
			}
			set 
			{
				this.target = value; 
			}
		}
		
		public bool IsPropagated ()
		{
			return this._isPropagated;
		}
		
		public GameEvent () {}
		
		public GameEvent (IEventDispatcher target)
		{
			this.Target = target;
		}
		
		public void StopPropagation ()
		{
			this._isPropagated = false;
		}
	}

}	
