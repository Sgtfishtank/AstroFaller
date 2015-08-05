using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

	private static GameObject _thisObject;
	public GameObject THISOBJECT;
	// Use this for initialization
	void Start ()
	{
		_thisObject = THISOBJECT;
		instance = _thisObject.GetComponent<GlobalVariables>();
		DontDestroyOnLoad(this.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private static GlobalVariables instance = null;
	public static GlobalVariables Instance
	{
		get
		{
			if (instance == null)
			{
				instance = _thisObject.GetComponent<GlobalVariables>();
			}
			return instance;
		}
	}
}
