using UnityEngine;
using System.Collections;

namespace Festi.Event
{

	public class GameEvent 
	{
		private bool _isPropagated = true;
		protected System.Object target;
		public System.Object Target
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
			this.Target = (System.Object) target;
		}

		public GameEvent (System.Object target)
		{
			this.Target = target;
		}

		public GameEvent (string target)
		{
			this.Target = (System.Object) target;
		}

		public void StopPropagation ()
		{
			this._isPropagated = false;
		}
	}

}	
