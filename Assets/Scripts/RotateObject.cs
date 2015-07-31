using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	public int mDir;
	public float mSpeed;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (Vector3.forward, transform.rotation.x + mSpeed*Time.deltaTime);
	}
}
