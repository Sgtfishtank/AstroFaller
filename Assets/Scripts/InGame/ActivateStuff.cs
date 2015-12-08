using UnityEngine;
using System.Collections;

public class ActivateStuff : MonoBehaviour 
{
	private Transform[] go;

    void Awake()
    {
        go = gameObject.GetComponentsInChildren<Transform>();
        enabled = false;
    }

	// Use this for initialization
	void Start ()
	{
	}

	void OnDisable()
    {
        enabled = false;
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
	void Update () 
    {
	
	}
}
