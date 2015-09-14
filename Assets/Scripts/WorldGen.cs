using UnityEngine;
using System.Collections;

public class WorldGen : MonoBehaviour
{
	public static InGame Instance
	{
		get
		{
			return InGame.Instance;
		}
	}

	public Segment[] mSegments;
	public int[] mSegmentsTimesGenerated;
	public int[] mSegmentWeightDynamic;
	private Segment mCurrentSegment;
	private Segment mNextSegment;
	private float mCurrentPos;

	private float mOffset;
	private float mNoiseFactor;

	private Player mPlayer;
	private int mA;

	void Awake()
	{
		enabled = false;
	}

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if(mNextSegment.transform.position.y > mPlayer.CenterPosition().y)
		{
			NextSegment();
		}
	}

	int PickSegment()
	{
		int sum = 0;
		for (int i = 0; i < mSegmentWeightDynamic.Length; i++)
		{
			sum += mSegmentWeightDynamic[i];
		}

		int valie = UnityEngine.Random.Range (0, sum);

		int retval = -1;

		int valueToCompare = 0;
		for (int i = 0; i < mSegmentWeightDynamic.Length; i++)
		{
			valueToCompare += mSegmentWeightDynamic[i];
			if ((valueToCompare > valie) && (retval == -1))
			{
				mSegmentWeightDynamic[i] = mSegments[i].mStaticWeight;
				retval = i;
			}
			else
			{
				mSegmentWeightDynamic[i] += mSegments[i].mStaticWeight;
			}
		}

		return retval;
	}

	void NextSegment ()
	{
		Destroy(mCurrentSegment.gameObject);

		mCurrentSegment = mNextSegment;
		mNextSegment = SpawnSegment(PickSegment());
	}

	void SpawnStartSegments()
	{
		mCurrentSegment = SpawnSegment(1);
		mNextSegment = SpawnSegment(PickSegment());
	}

	Segment SpawnSegment(int index)
	{
		Segment segmentPrefab = mSegments[index];
		float segSize = mOffset * segmentPrefab.mLength;
		mCurrentPos -= segSize;
		Vector3 pos = new Vector3 (0, mCurrentPos + (segSize * 0.5f), 0);
		Segment newSeg = Instantiate(segmentPrefab, pos, Quaternion.identity) as Segment;
		
		newSeg.transform.position += segmentPrefab.transform.position;

		if (mNoiseFactor > 0)
		{
			newSeg.transform.position += Random.insideUnitSphere * mNoiseFactor;
			newSeg.transform.localScale = segmentPrefab.transform.localScale * (1f - (mNoiseFactor / 400) + (Random.value * (mNoiseFactor / 200)));
		}
		
		mSegmentsTimesGenerated[index]++;
		newSeg.transform.parent = transform;
		return newSeg;
	}

	public void DespawnSegments() 
	{
		if (mNextSegment != null)
		{
			Destroy(mNextSegment.gameObject);
		}
		
		if (mCurrentSegment != null)
		{
			Destroy(mCurrentSegment.gameObject);
		}

		mNextSegment = null;
		mCurrentSegment = null;
	}

	public void ShiftBack (float shift)
	{
		mCurrentPos -= shift;
		if(mCurrentSegment != null)
		{
			mCurrentSegment.transform.position -= new Vector3(0, shift, 0);
			mNextSegment.transform.position -= new Vector3(0, shift, 0);
		}
	}
	
	public void UnloadSegments()
	{
		DespawnSegments ();
		mSegments = null;
	}

	public void LoadSegments(string path, float baseSize, float noiseFactor)
	{
		mNoiseFactor = noiseFactor;
		mOffset = baseSize;

		mPlayer = InGame.Instance.mPlayer;

		mSegments = Resources.LoadAll<Segment>(path) as Segment[];
		mSegmentWeightDynamic = new int[mSegments.Length];
		mSegmentsTimesGenerated = new int[mSegments.Length];
		for (int i = 0; i < mSegments.Length; i++)
		{
			mSegmentWeightDynamic[i] = mSegments[i].mStaticWeight;
		}
	}

	public void StartSpawnSegments(float startSpawnY)
	{
		enabled = true;
		mCurrentPos = startSpawnY;
		SpawnStartSegments ();
	}

	public void StopSpawnSegments()
	{
		enabled = false;
	}
}
