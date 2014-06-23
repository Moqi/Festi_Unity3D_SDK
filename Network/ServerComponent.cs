using UnityEngine;
using System.Collections;
using System.Reflection;
using Festi;
using Festi.Network.Model;
using Festi.Event;

namespace Festi.Network
{

	[AddComponentMenu("Festi/Network/ServerComponent")]
	public class ServerComponent : SceneComponent
	{
		const string EVENT_CONNECT = "OnConnect";

		[SerializeField] protected string host = "localhost";
		[SerializeField] protected int port = 2010;
		[SerializeField] protected string modelName = "Default";

		private DefaultModel _model;

		public override void OnInit()
		{
			this._InitModel();
		}

		private void _InitModel()
		{
			string modelClassName = "Festi.Network.Model." + modelName + "Model";
			System.Type modelClass = System.Type.GetType(modelClassName);
			this._model = (DefaultModel) System.Activator.CreateInstance(modelClass);

			this._model.AddListener("OnConnect", OnConnect);
		} // end _InitModel

		public void Connect()
		{
			this._model.Connect(this.host, this.port);
		} // end Connect

		public virtual void OnConnect(GameEvent evt)
		{
			this.Dispatch(ServerComponent.EVENT_CONNECT, evt);
		}

    }
}