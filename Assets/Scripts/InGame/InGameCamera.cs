using UnityEngine;
using System.Collections;

public class InGameCamera : MonoBehaviour 
{
	public GameObject crash = null;
	// snigleton
	private static InGameCamera instance = null;
	public static InGameCamera Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("InGame Camera");
				instance = thisObject.GetComponent<InGameCamera>();
			}
			return instance;
		}
	}

	void Awake()
	{
	}

	void OnEnable()
	{
		//crash.transform.position = Vector3.zero;
	}
	
	void OnDisable()
	{
		//crash.transform.position = Vector3.zero;
	}

	// Use this for initialization
	void Start () 
	{
	}

	// Update is called once per frame
	void Update () 
	{
	}
}
