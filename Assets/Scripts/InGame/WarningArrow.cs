using UnityEngine;
using System.Collections;

public class WarningArrow : MonoBehaviour 
{
	public GameObject mWarningPrefab;
	
	private GameObject mWarning;
	private float mHideT;

	void Awake()
	{
		mWarning = GlobalVariables.Instance.Instanciate (mWarningPrefab, null, 0.05f);
		mWarning.transform.parent = transform;
		mHideT = Time.time + GlobalVariables.Instance.ASTEROID_WARNING_MAX_SHOW_TIME;
	}

	// Use this for initialization
	void Start () 
	{
	}

	void OnDisable()
	{
		mWarning.SetActive(false);
	}

	void OnEnable()
	{
		mWarning.SetActive(true);
		mHideT = Time.time + GlobalVariables.Instance.ASTEROID_WARNING_MAX_SHOW_TIME;
	}

	// Update is called once per frame
	void Update () 
	{
		Player mPlayer = InGame.Instance.Player();
		Rigidbody mRb = GetComponent<Rigidbody>();

		Vector3 plVel = mPlayer.Rigidbody().velocity;
		
		Quaternion rot = Quaternion.LookRotation (new Vector3(0, 0, 1), mRb.velocity - new Vector3(0, plVel.y, 0));
		mWarning.transform.rotation = rot * Quaternion.Euler (0, 0, 90);
		
		
		float minX = InGameCamera.Instance.Camera().WorldToScreenPoint(new Vector3(-GlobalVariables.Instance.PLAYER_MINMAX_X, mPlayer.CenterPosition().y, mPlayer.CenterPosition().z)).x;
		float maxX = InGameCamera.Instance.Camera().WorldToScreenPoint(new Vector3(GlobalVariables.Instance.PLAYER_MINMAX_X, mPlayer.CenterPosition().y, mPlayer.CenterPosition().z)).x;

        float minY = InGameCamera.Instance.Camera().WorldToScreenPoint(new Vector3(mPlayer.CenterPosition().x, mPlayer.CenterPosition().y + GlobalVariables.Instance.PLAYER_MIN_Y, mPlayer.CenterPosition().z)).y;
        float maxY = InGameCamera.Instance.Camera().WorldToScreenPoint(new Vector3(mPlayer.CenterPosition().x, mPlayer.CenterPosition().y + GlobalVariables.Instance.PLAYER_MAX_Y, mPlayer.CenterPosition().z)).y;

		Vector3 a = InGameCamera.Instance.Camera().WorldToScreenPoint(mRb.position);
		
		a.z = 1;

        bool outOfBoundX = ((mRb.position.x < 0) && (a.x > minX)) || ((mRb.position.x > 0) && (a.x < maxX));
        bool outOfBoundY = ((mRb.position.y < 0) && (a.y > minY)) || ((mRb.position.y > 0) && (a.y < maxY));

		if ((outOfBoundX && outOfBoundY) || (mHideT < Time.time))
		{
            mWarning.SetActive(false);
            return;
		}

        if (outOfBoundY)
        {
            if (mRb.position.x < 0)
            {
                a.x = minX;
            }
            else
            {
                a.x = maxX;
            } 
        }
        else if (outOfBoundX)
        {
            if (mRb.position.y < 0)
            {
                a.y = (minY);
            }
            else
            {
                a.y = (maxY);
            } 
        }
		
		mWarning.transform.position = InGameCamera.Instance.Camera().ScreenToWorldPoint(a);
	}
}
