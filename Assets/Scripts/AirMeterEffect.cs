using UnityEngine;
using System.Collections;

// Thism2 created 2015-04-23 : air meter effect
// Thism2 edited 2015-04-29 : added that air meter hides if full to long
// Thism2 edited 2015-05-11 : added reset function
public class AirMeterEffect : MonoBehaviour 
{
	public bool mMirrored;
	public float mHideShowDelay;
	public GameObject mParticleSystemPrefab;
	public int mArcSize;
	public float mSmoothSpeed;

	private Player mPlayer;
	private GameObject mAirMeterHolder;
	private ParticleSystem[] mParticleSystemArcs;
	private float mHideTime;
	private float mShowTime;

	// Thism2 created 2015-04-23 : Use this for initialization
	void Start () 
	{
		int sum = (360 / Mathf.Max(mArcSize, 1)); // to protect from x / 0 error
		mParticleSystemArcs = new ParticleSystem[sum];
		mAirMeterHolder = transform.Find ("AirMeter").gameObject;

		// create all parts of the meter
		for (int i = 0; i < mParticleSystemArcs.Length; i++) 
		{
			GameObject obj = (GameObject)GameObject.Instantiate(mParticleSystemPrefab);
			obj.SetActive (true);
			obj.transform.parent = mAirMeterHolder.transform;
			obj.transform.localPosition = new Vector3(0, 0, 0);
			mParticleSystemArcs[i] = obj.GetComponent<ParticleSystem>();
		}

		mPlayer = GetComponent<Player>();

		mHideTime = 0;
		mShowTime = 0;
	}
	
	// Thism2 created 2015-05-11 : Resets the air meter
	public void reset()
	{
		for (int i = 0; i < mParticleSystemArcs.Length; i++) 
		{
			mParticleSystemArcs [i].Stop ();
		}
	}
	
	// Thism2 created 2015-05-25 : Updates the position to match onto the camera screen (should be called after camera move)
	public void updatePosition(Camera camera)
	{
		if (camera == null) 
		{
			return;
		}

		// set approiate rotation according to camera
		mAirMeterHolder.transform.rotation = camera.transform.rotation;

		// set approiate position according to camera
		Vector3 pos = camera.WorldToScreenPoint(mPlayer.transform.position);
		pos.z = Mathf.Abs(camera.transform.position.z - mPlayer.transform.position.z) / 2f;
		Vector3 realpos = camera.ScreenToWorldPoint(pos);
		mAirMeterHolder.transform.position = Vector3.Slerp(mAirMeterHolder.transform.position, realpos, Time.deltaTime * mSmoothSpeed); 
	}

	// Thism2 created 2015-04-23 : Update is called once per frame
	void Update () 
	{
		float value2 = ((float)mPlayer.AirAmount()) / ((float)GlobalVariables.Instance.PLAYER_MAX_AIR);

		// hide meter if it is full
		if (mPlayer.AirAmount() < GlobalVariables.Instance.PLAYER_MAX_AIR)
		{
			mHideTime = Time.time + mHideShowDelay;
		}

		// show meter if no longer full
		if (mPlayer.AirAmount() == GlobalVariables.Instance.PLAYER_MAX_AIR)
		{
			mShowTime = Time.time + mHideShowDelay;
		}

		// check all parts of the meter 
		for (int i = 0; i < mParticleSystemArcs.Length; i++) 
		{
			float angle = mArcSize * i;
			ParticleSystem PopSys = mParticleSystemArcs[i];

			// choose meter depletion direction
			if (mMirrored)
			{
				PopSys.transform.rotation = Quaternion.Euler(0, 0, 90f - mArcSize - angle);
			}
			else
			{
				PopSys.transform.rotation = Quaternion.Euler(0, 0, 90f + angle);
			}

			// turn parts on and off
			if (angle >= (value2 * 360f))
			{
				if (PopSys.isPlaying)
				{
					PopSys.Stop();
				}
			}
			else if (mShowTime <= Time.time)
			{
				if (!PopSys.isPlaying)
				{
					PopSys.Play();
				}
			}
			else if (mHideTime <= Time.time)
			{
				if (PopSys.isPlaying)
				{
					PopSys.Stop();
				}
			}
		}
	}
}
