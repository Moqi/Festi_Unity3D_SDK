using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Festi
{
	public abstract class SceneController : FestiMonoBehaviour 
	{	
		private static SceneController _instance;
			
		public static SceneController GetInstance()
		{
			if (_instance == null) {
				throw new System.Exception("Undefined SceneController");
			}

			return _instance;
		} // end GetInstance

		public void Awake()
		{
			_instance = this;
		} // end Awake

		public virtual void Start()
		{	
			_instance = this;
			this.OnInit();

			List<SceneComponent> components = this.GetAllSceneComponents();
			this._InitComponents(components);

			this.OnStart();

			this._StartComponents(components);
		} // end Start

		protected virtual void OnInit()
		{
		}

		protected virtual void OnStart()
		{
		}

		private void _InitComponents(List<SceneComponent> components)
		{
			foreach (SceneComponent component in components) {
				component.OnInit();
			}
		}

		private void _StartComponents(List<SceneComponent> components)
		{
			foreach (SceneComponent component in components) {
				component.OnStart();
			}
		}

		public SceneComponent GetSceneComponent(string name)
		{
			return (SceneComponent) GetComponentInChildren(System.Type.GetType(name));	
		}
		
		public List<SceneComponent> GetAllSceneComponents()
		{
			return new List<SceneComponent>(GetComponentsInChildren<SceneComponent>());
		} 
	}
}