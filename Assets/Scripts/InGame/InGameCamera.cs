using UnityEngine;
using System.Collections;

public class InGameCamera : MonoBehaviour 
{
	public GameObject crash = null;
	// snigleton
	private static InGameCamera instance = null;
	public static InGameCamera Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("InGame Camera");
				instance = thisObject.GetComponent<InGameCamera>();
			}
			return instance;
		}
	}
	
	private TextMesh mDistnceText;
	private TextMesh mBoltsText;
	private TextMesh mBoxesText;
	private TextMesh mLifeText;
	private Camera mCamera;

	void Awake()
	{
		mCamera = GetComponent<Camera> ();
		mBoltsText = transform.Find ("UI/Bolt_Count_text").GetComponent<TextMesh> ();
		mDistnceText = transform.Find ("UI/Distance_Text").GetComponent<TextMesh> ();
		mBoxesText = transform.Find ("UI/Perfect_Distance_Boxes/Amount_Of_Boxes_Text").GetComponent<TextMesh> ();
		mLifeText = transform.Find ("UI/Life_Count_text").GetComponent<TextMesh> ();

	}

	void OnEnable()
	{
		GetComponent<FollowPlayer> ().ydist = 0;
	}
	
	void OnDisable()
	{
		//crash.transform.position = Vector3.zero;
	}

	// Use this for initialization
	void Start () 
	{
	}

	// Update is called once per frame
	void Update () 
	{
		int temp = InGame.Instance.Player().colectedBolts();
		if(temp >= 10000)
			mBoltsText.text = (temp/1000).ToString()+ "K";
		else
			mBoltsText.text = (temp).ToString();
		temp = InGame.Instance.Player().distance();
		if(temp >= 1000000)		
			mDistnceText.text = (temp/1000000).ToString() + "M";
		else if(temp >= 10000)
			mDistnceText.text = (temp/1000).ToString() + "K";
		else
			mDistnceText.text = temp.ToString();
		temp = InGame.Instance.Player().CollectedPerfectDistances();
		if(temp >= 1000)
			mBoxesText.text = (temp/1000).ToString()+ " K";
		else
			mBoxesText.text = temp.ToString();
		mLifeText.text = InGame.Instance.Player().LifeRemaining().ToString();
	}

	public Camera Camera()
	{
		return mCamera;
	}
}
