using UnityEngine;
using System.Collections;

public class PopupBuyMenu : MonoBehaviour 
{
	//private Buyable mBuyable;

	private TextMesh mDescriptionText;
	private TextMesh mCurrentText;
	private TextMesh mNextText;
	private TextMesh mCostBoltsText;
	private TextMesh mCostCrystalsText;

	// Use this for initialization
	void Start () 
	{
		mDescriptionText = transform.Find ("name ext").GetComponent<TextMesh> ();
		mCurrentText = transform.Find ("info text 1").GetComponent<TextMesh> ();
		mNextText = transform.Find ("info text 2").GetComponent<TextMesh> ();
		mCostBoltsText = transform.Find ("buy text 1").GetComponent<TextMesh> ();
		mCostCrystalsText = transform.Find ("buy text 2").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if (mReferance)
		{
			//mBuyable
		}

		/*mDescriptionText = GlobalVariables.Instance.BuyDescription();
		mCurrentText = "CURRENT: " + GlobalVariables.Instance.BuyCurrent();
		mNextText = "NEXT: " + GlobalVariables.Instance.BuyNext();
		mCostBoltsText = GlobalVariables.Instance.BuyCostBolts() + " B";
		mCostCrystalsText = GlobalVariables.Instance.BuyCostCrystals() + " C";*/
	}


}
