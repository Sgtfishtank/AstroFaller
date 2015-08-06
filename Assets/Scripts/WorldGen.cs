using UnityEngine;
using System.Collections;

public class WorldGen : MonoBehaviour
{
	public GameObject[] mSegments;
	private GameObject mCurrentSegment;
	private GameObject mNextSegment;
	public GameObject mPlayer;
	private bool mFirstTime = true;
	private float mCurrentPos =-25;
	private float mOffset = 50;

	// Use this for initialization
	void Start ()
	{
		mSegments = Resources.LoadAll<GameObject>("Level1") as GameObject[];

		mCurrentSegment = Instantiate(mSegments[1], new Vector3(0,mCurrentPos,0), Quaternion.identity) as GameObject;
		mPlayer = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mCurrentPos > mPlayer.transform.position.y && !mFirstTime)
		{
			Destroy(mCurrentSegment);
			mCurrentSegment = mNextSegment;
			mNextSegment = Instantiate(mSegments[UnityEngine.Random.Range(0,mSegments.Length)],
			                           new Vector3 (0,mCurrentPos-mOffset,0),Quaternion.identity) as GameObject;
			mCurrentPos -= mOffset;
		}

		else if (mCurrentPos > mPlayer.transform.position.y)
		{
			mFirstTime = false;
			mNextSegment = Instantiate(mSegments[UnityEngine.Random.Range(0,mSegments.Length)],
			                           new Vector3 (0,mCurrentPos -mOffset,0),Quaternion.identity) as GameObject;
			mCurrentPos -= mOffset;
		}
	
	}
}
