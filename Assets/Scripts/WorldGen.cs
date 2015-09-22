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

	public Segment[] mSegments;
	public int[] mSegmentsTimesGenerated;
	public int[] mSegmentWeightDynamic;
	private Segment mCurrentSegment;
	private Segment mNextSegment;
	private float mCurrentPos;
	private int mLastSegmentNr;

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
				mSegmentWeightDynamic[i] = mSegments[i*2].mStaticWeight;
				retval = i*2;
			}
			else
			{
				mSegmentWeightDynamic[i] += mSegments[i*2].mStaticWeight;
			}
		}
		mLastSegmentNr = retval;
		if(mSegments[retval].gameObject.activeSelf)
			retval++;
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
		mCurrentSegment = SpawnSegment(2);
		mNextSegment = SpawnSegment(PickSegment());
	}

	Segment SpawnSegment(int index)
	{
		Segment segmentPrefab = mSegments[index];
		float segSize = mOffset * segmentPrefab.mLength;
		mCurrentPos -= segSize;
		Vector3 pos = new Vector3 (0, mCurrentPos + (segSize * 0.5f), segmentPrefab.transform.position.z);
		Segment newSeg = segmentPrefab;
		newSeg.transform.position = pos;
		newSeg.gameObject.SetActive(true);

		GameObject[] bolts = newSeg.GetComponentsInChildren<Transform>(true).Where(x => x.tag == "Bolts").Select(x => x.gameObject).ToArray();

		for (int i = 0; i < bolts.Length; i++) 
		{
			bolts[i].SetActive(true);
		}

		/*if (mNoiseFactor > 0)
		{
			newSeg.transform.position += Random.insideUnitSphere * mNoiseFactor;
			newSeg.transform.localScale = segmentPrefab.transform.localScale * (1f - (mNoiseFactor / 400) + (Random.value * (mNoiseFactor / 200)));
		}*/
		
		mSegmentsTimesGenerated[index/2]++;

		return newSeg;
	}

	public void DespawnSegments() 
	{
		/*if (mNextSegment != null)
		{
			Destroy(mNextSegment.gameObject);
		}
		
		if (mCurrentSegment != null)
		{
			Destroy(mCurrentSegment.gameObject);
		}*/
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
	}

	public void LoadSegments(string path, float baseSize, float noiseFactor)
	{
		mNoiseFactor = noiseFactor;
		mOffset = baseSize;

		mPlayer = InGame.Instance.mPlayer;


		Segment[] s = Resources.LoadAll<Segment>(path) as Segment[];
		mSegments = new Segment[s.Length*2];
		for (int i = 0; i < s.Length; i++)
		{
			GameObject g1 = Instantiate(s[i].gameObject, new Vector3(0,100,s[i].transform.position.z), Quaternion.identity) as GameObject;
			GameObject g2 = Instantiate(s[i].gameObject, new Vector3(0,100,s[i].transform.position.z), Quaternion.identity) as GameObject;
			//print(g1.GetComponent<Segment>() == null);
			mSegments[i*2] = g1.GetComponent<Segment>();
			mSegments[i*2].gameObject.SetActive(false);
			mSegments[i*2+1] =g2.GetComponent<Segment>();
			mSegments[i*2+1].gameObject.SetActive(false);
			mSegments[i*2].transform.parent = transform;
			mSegments[i*2+1].transform.parent = transform;
		}
		print(mSegments[1] == null);

		mSegmentWeightDynamic = new int[s.Length];
		mSegmentsTimesGenerated = new int[s.Length];
		for (int i = 0; i < s.Length; i++)
		{
			mSegmentWeightDynamic[i] = s[i].mStaticWeight;
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
