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
	private Camera mCamera;

	void Awake()
	{
		mCamera = GetComponent<Camera> ();
		mBoltsText = transform.Find ("UI/Bolt_Count_text").GetComponent<TextMesh> ();
		mDistnceText = transform.Find ("UI/Distance_Text").GetComponent<TextMesh> ();
		mBoxesText = transform.Find ("UI/Perfect_Distance_Boxes/Amount_Of_Boxes_Text").GetComponent<TextMesh> ();
	}

	void OnEnable()
	{
		//crash.transform.position = Vector3.zero;
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
	}

	public Camera Camera()
	{
		return mCamera;
	}
}
