using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	public Transform mTrans;
	public int mDir;
	public float mSpeed;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		mTrans.Rotate (Vector3.forward, mTrans.rotation.x + mSpeed*Time.deltaTime);
	}
}
