using UnityEngine;
using System.Collections;

public class InGameCamera : MonoBehaviour 
{
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

	public GameObject mWarningPrefab;
	private GameObject mWarning;

	void Awake()
	{
		mWarning = transform.Find ("Warning").gameObject;

		GameObject g3 = GlobalVariables.Instance.Instanciate (mWarningPrefab, mWarning.transform, 0.05f);

		mWarning.SetActive (true);
	}

	// Use this for initialization
	void Start () 
	{
		InGameCamera.Instance.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{
		if (WorldGen.Instance.mPlayer.GetComponent<Player>().mAS.GetComponent<AstroidSpawn>().mAstroids.Count < 1)
		{
			mWarning.SetActive(false);
			return;
		}
		mWarning.SetActive(true);

		GameObject ast = WorldGen.Instance.mPlayer.GetComponent<Player>().mAS.GetComponent<AstroidSpawn>().mAstroids[0];
		GameObject ply = WorldGen.Instance.mPlayer;
		Vector3 plPos = ply.transform.position + (ply.transform.rotation * ply.GetComponent<Rigidbody>().centerOfMass);
		Vector3 diff = plPos - ast.transform.position;

		Vector3 pos = ast.transform.position;
		//pos.z -= 15f;

		Quaternion rot2 = Quaternion.Euler (0, 0, 90);

		Quaternion rot = Quaternion.LookRotation (mWarning.transform.forward, diff);
		mWarning.transform.rotation = rot * rot2;

		Vector3 a = GetComponent<Camera> ().WorldToScreenPoint(pos);

		if (a.x < Screen.width / 2)
		{
			a.x = Mathf.Clamp (a.x, 10, 11);
		}
		else
		{
			a.x = Mathf.Clamp (a.x, Screen.width - 11, Screen.width - 10);
		}

		a.y = Mathf.Clamp (a.y, 10, Screen.height - 10);
		a.z = 1;

		mWarning.transform.position = GetComponent<Camera> ().ScreenToWorldPoint(a);
	}

	public void showWarning(bool show)
	{
		mWarning.SetActive (show);
	}
}
