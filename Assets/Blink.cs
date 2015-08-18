using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour 
{
	public Light mLight;
	public float mBlinkTime;
	public float mTime;

	// Use this for initialization
	void Start () 
	{
		mLight = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > mTime)
		{
			mTime += mBlinkTime;
			mLight.enabled = !mLight.enabled;
		}
	}
}
