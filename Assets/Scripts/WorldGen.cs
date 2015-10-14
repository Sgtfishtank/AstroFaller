using UnityEngine;
using System.Collections;
using System.Linq;

public class WorldGen : MonoBehaviour
{
	public static InGame Instance
	{
		get
		{
			return InGame.Instance;
		}
	}
	
	public Segment[] mSegmentPrefabs;
	public Segment[] mSegments;
	public int[] mSegmentsTimesGenerated;
	public int[] mSegmentWeightDynamic;
	private Segment mCurrentSegment;
	private Segment mNextSegment;
	private float mCurrentPos;
	//private int mLastSegmentNr;

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
				mSegmentWeightDynamic[i] = mSegmentPrefabs[i].mStaticWeight;
				retval = i;
			}
			else
			{
				mSegmentWeightDynamic[i] += mSegmentPrefabs[i].mStaticWeight;
			}
		}
		//mLastSegmentNr = retval;
		return retval;
	}

	void NextSegment ()
	{
		//Destroy(mCurrentSegment.gameObject);
		mCurrentSegment.gameObject.SetActive(false);

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
		Segment segmentPrefab = mSegmentPrefabs[index];
		
		Segment newSeg = mSegments[index * 2];
		if(mSegments[index * 2].gameObject.activeSelf)
		{
			newSeg = mSegments[(index * 2) + 1];
		}

		float segSize = mOffset * segmentPrefab.mLength;
		mCurrentPos -= segSize;
		Vector3 pos = new Vector3 (0, mCurrentPos + (segSize * 0.5f), 0);
		newSeg.transform.position = pos + segmentPrefab.transform.position;
		newSeg.transform.rotation = segmentPrefab.transform.rotation;
		newSeg.gameObject.SetActive(true);

		GameObject[] bolts = newSeg.GetComponentsInChildren<Transform>(true).Where(x => x.tag == "Bolts").Select(x => x.gameObject).ToArray();

		for (int i = 0; i < bolts.Length; i++) 
		{
			bolts[i].SetActive(true);
		}

		if (mNoiseFactor > 0)
		{
			Vector3 randPos = Random.insideUnitCircle;

			newSeg.transform.position += randPos * mNoiseFactor;
			newSeg.transform.rotation = Quaternion.Euler(newSeg.transform.rotation.eulerAngles + new Vector3(0, 0, (Random.value - 0.5f) * mNoiseFactor * 10));
			newSeg.transform.localScale = segmentPrefab.transform.localScale * (1f - (mNoiseFactor / 400) + (Random.value * (mNoiseFactor / 200)));

		}
		
		mSegmentsTimesGenerated[index]++;

		return newSeg;
	}

	public void DespawnSegments() 
	{
		for (int i = 0; i < mSegments.Length; i++)
		{
			mSegments[i].gameObject.SetActive(false);
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
		for (int i = 0; i < mSegments.Length; i++)
		{
			Destroy(mSegments[i].gameObject);
		}
		mSegments = null;
		mSegmentPrefabs = null;
	}

	public void LoadSegments(string path, float baseSize, float noiseFactor)
	{
		mNoiseFactor = noiseFactor;
		mOffset = baseSize;

		mPlayer = InGame.Instance.mPlayer;

		mSegmentPrefabs = Resources.LoadAll<Segment>(path) as Segment[];
		mSegments = new Segment[mSegmentPrefabs.Length*2];
		for (int i = 0; i < mSegmentPrefabs.Length; i++)
		{
			GameObject g1 = Instantiate(mSegmentPrefabs[i].gameObject, new Vector3(0,100, mSegmentPrefabs[i].transform.position.z), Quaternion.identity) as GameObject;
			GameObject g2 = Instantiate(mSegmentPrefabs[i].gameObject, new Vector3(0,100, mSegmentPrefabs[i].transform.position.z), Quaternion.identity) as GameObject;
			mSegments[i*2] = g1.GetComponent<Segment>();
			mSegments[i*2].gameObject.SetActive(false);
			mSegments[i*2+1] =g2.GetComponent<Segment>();
			mSegments[i*2+1].gameObject.SetActive(false);
			mSegments[i*2].transform.parent = transform;
			mSegments[i*2+1].transform.parent = transform;
		}

		mSegmentWeightDynamic = new int[mSegmentPrefabs.Length];
		mSegmentsTimesGenerated = new int[mSegmentPrefabs.Length];
		for (int i = 0; i < mSegmentPrefabs.Length; i++)
		{
			mSegmentWeightDynamic[i] = mSegmentPrefabs[i].mStaticWeight;
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
