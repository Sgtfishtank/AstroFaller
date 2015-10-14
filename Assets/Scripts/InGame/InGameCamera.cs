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
		mBoltsText.text = InGame.Instance.Player().colectedBolts().ToString();
		mDistnceText.text = InGame.Instance.Player().distance().ToString();
		mBoxesText.text = InGame.Instance.Player().CollectedPerfectDistances().ToString();
		mLifeText.text = InGame.Instance.Player().LifeRemaining().ToString();
	}

	public Camera Camera()
	{
		return mCamera;
	}
}
