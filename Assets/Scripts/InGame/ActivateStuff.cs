using UnityEngine;
using System.Collections;

public class ActivateStuff : MonoBehaviour {

	public Transform[] go;
	// Use this for initialization
	void Start ()
	{
		go = gameObject.GetComponentsInChildren<Transform>();
	}
	void OnDisable()
	{
		for (int i = 1; i < go.Length; i++)
		{
			go[i].gameObject.SetActive(false);
		}
	}
	void OnEnable()
	{
		for (int i = 1; i < go.Length; i++)
		{
			go[i].gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
