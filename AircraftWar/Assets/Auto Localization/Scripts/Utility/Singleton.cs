using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{

	private static T _instance;
	public string InstanceID;
	// public static List<MonoBehaviour> Singletons = new List<MonoBehaviour>();

	public static T instance {
		get {
			if (ReferenceEquals (_instance, null)) {
				_instance = GameObject.FindObjectOfType<T> ();
//                this.InstanceID = _instance.GetInstanceID();
				//Tell unity not to destroy this object when loading a new scene!
				if (_instance != null) {
					
					DontDestroyOnLoad (_instance.gameObject);
				} else {
					Debug.Log ("<color=green> The Object  " + typeof(T) + " Not Created as Singleton </color>");
				}

			}
		
			return _instance;
		}
	}

	public static bool HasInstance {
		get {
			return _instance != null;
		}
	}

//    public static void AddInstance(MonoBehaviour i)
//    {
//        Singletons.Add(i);
//    }

	protected virtual void Awake ()
	{
		if (_instance == null) {
			//If I am the first instance, make me the Singleton
			_instance = this as T;
			// AddInstance(_instance);
			InstanceID = _instance.GetInstanceID ().ToString ();
			DontDestroyOnLoad (this);
		} else {
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
//			if (this != _instance)
			Debug.LogError ("[Singelton]Destroying: " + this.gameObject.name + " " + "New InstanceID :" + this.GetInstanceID () + " Old :" + _instance.GetInstanceID ());
			Destroy (this.gameObject);
		}
	}

	void OnDestroy ()
	{
		// _instance = null;
	}
}