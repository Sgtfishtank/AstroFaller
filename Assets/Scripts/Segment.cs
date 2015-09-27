using UnityEngine;
using System.Collections;
using System.Linq;

public class Segment : MonoBehaviour 
{
	public int mStaticWeight;
	public int mLength;
	public Vector3[] mOriginalPosition;
	public Quaternion[] mOriginalRotation;

	// Use this for initialization
	void Start () 
	{
	}
	void OnDisable()
	{
		if(mOriginalPosition.Length == 0)
		{
			Transform[] b = gameObject.GetComponentsInChildren<Transform>(true).Where(x =>x.tag == "Enemy").ToArray();
			mOriginalPosition = new Vector3[b.Length];
			mOriginalRotation = new Quaternion[b.Length];
			for (int i = 0; i < b.Length; i++)
			{
				mOriginalPosition[i] = b[i].localPosition;
				mOriginalRotation[i] = b[i].localRotation;
			}
		}

		Transform[] a = gameObject.GetComponentsInChildren<Transform>(true).Where(x =>x.tag == "Enemy").ToArray();

		for (int i = 0; i < a.Length; i++)
		{
			a[i].localPosition = mOriginalPosition[i];
			a[i].localRotation = mOriginalRotation[i];
			if(a[i].gameObject.GetComponent<Rigidbody>() != null)
				a[i].gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
