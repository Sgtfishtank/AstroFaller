using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	public int mDir;
	public float mSpeed;
	public int x = 0;
	public int y = 0;
	public int z = 0;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate ( new Vector3(x,y,z), mSpeed*Time.deltaTime);
	}
}
