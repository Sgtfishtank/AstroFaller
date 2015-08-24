using UnityEngine;
using System.Collections;

public class PopupBuyMenu : MonoBehaviour 
{
	public GameObject mPrefab;

	private bool mOpen;
	private TextMesh mDescriptionText;
	private TextMesh mCurrentText;
	private TextMesh mNextText;
	private TextMesh mCostBoltsText;
	private TextMesh mCostCrystalsText;

	void Awake()
	{
		GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		
		mDescriptionText = transform.Find ("Pop-up buy menu/name ext").GetComponent<TextMesh> ();
		mCurrentText = transform.Find ("Pop-up buy menu/info text 1").GetComponent<TextMesh> ();
		mNextText = transform.Find ("Pop-up buy menu/info text 2").GetComponent<TextMesh> ();
		mCostBoltsText = transform.Find ("Pop-up buy menu/buy text 1").GetComponent<TextMesh> ();
		mCostCrystalsText = transform.Find ("Pop-up buy menu/buy text 2").GetComponent<TextMesh> ();

		mOpen = false;
	}

	// Use this for initialization
	void Start()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public void Open()
	{
		mOpen = true;
		gameObject.SetActive(true);

		GUICanvas.Instance.ShowPopupBuyButton (true);
	}

	public void Close()
	{
		mOpen = false;
		gameObject.SetActive(false);
		GUICanvas.Instance.ShowPopupBuyButton (false);
	}

	public bool IsOpen ()
	{
		return mOpen;
	}

	public void updateData (string description, string current, string next, int costBolts, int nextCrystals)
	{
		mDescriptionText.text = description;
		mCurrentText.text = "CURRENT: " + current;
		mNextText.text = "NEXT: " + next;
		mCostBoltsText.text = costBolts + " B";
		mCostCrystalsText.text = nextCrystals + " C";
	}
}
