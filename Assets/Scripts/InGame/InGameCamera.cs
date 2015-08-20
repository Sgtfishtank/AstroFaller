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
		if (WorldGen.Instance.AstroidSpawn().mAstroids.Count < 1)
		{
			mWarning.SetActive(false);
			return;
		}
		mWarning.SetActive(true);

		GameObject ast = WorldGen.Instance.AstroidSpawn().mAstroids[0];
		Player ply = WorldGen.Instance.Player();

		Vector3 diff = ply.CenterPosition() - ast.GetComponent<Rigidbody>().position;

		Vector3 plVel = ply.GetComponent<Rigidbody> ().velocity;
		Quaternion rot = Quaternion.LookRotation (mWarning.transform.forward, ast.GetComponent<Rigidbody>().velocity - new Vector3(0, plVel.y, 0));
		mWarning.transform.rotation = rot * Quaternion.Euler (0, 0, 90);

		Vector3 a = GetComponent<Camera>().WorldToScreenPoint(ast.transform.position);
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
