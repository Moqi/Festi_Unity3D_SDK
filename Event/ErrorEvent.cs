using UnityEngine;
using System.Collections;

namespace Festi.Event
{
	public class ErrorEvent : GameEvent
	{
		public const string ERROR = "ERROR";
		
		private string _errorText;
		
		public string GetText ()
		{
			return _errorText;
		}
		
		public void SetText (string text)
		{
			this._errorText = text;	
		}
	}
}
