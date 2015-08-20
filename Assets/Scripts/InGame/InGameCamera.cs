using UnityEngine;
using System.Collections;

public class InGameCamera : MonoBehaviour 
{
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

	// Use this for initialization
	void Start () 
	{
		InGameCamera.Instance.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public void showWarning(bool show)
	{
		//mWarning.SetActive (show);
	}
}
