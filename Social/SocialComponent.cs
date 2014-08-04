using UnityEngine;
using System.Collections;

namespace Festi.Social
{	
	public class SocialComponent : Festi.SceneComponent 
	{
		public const string ON_API_INIT_COMPLETE = "api_init_complete";
		public const string ON_API_LOGIN_SUCCESS = "api_login_success";
		public const string ON_API_LOGIN_ERROR   = "api_login_error";
		public const string ON_METHOD_LOAD_ERROR = "api_method_load_error";
		public const string ON_LOAD_APP_FRIENDS_COMPLETE = "app_friends_complete";

		public delegate void CallbackDelegate(object result);

		public virtual void Login(string scope = "")
		{
		}

		public virtual void LoadAppFriends(string fields = "id,name", int limit = 100)
		{
		}

		public virtual void SendInviteRequest(string invateMessage = "")
		{
		}

		public virtual void LoadAvatar(SocialUser user, CallbackDelegate callback, int? width = null, int? height = null)
		{
		} // end LoadAvatar

	}
}

