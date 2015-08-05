using UnityEngine;
using System.Collections;

public class UnlockCriteria : MonoBehaviour 
{
	public Level mCriteriaLevel;

	private Level mLevel;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public bool CriteriaMet()
	{
		int bolts = GlobalVariables.Instance.BoltsCritera("Bonus 1");
		int Distance = GlobalVariables.Instance.DistanceCritera("Bonus 1");

		return (mCriteriaLevel.TotalBolts() >= bolts) && (mCriteriaLevel.TotalDistance() >= Distance);
	}


}
