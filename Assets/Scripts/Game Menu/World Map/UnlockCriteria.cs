using UnityEngine;
using System.Collections;

public class UnlockCriteria : MonoBehaviour 
{
	public Level mLevel;
	public int mTotalDistance;
	public int mTotalBolts;

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
		return (mLevel.TotalBolts() >= mTotalBolts) && (mLevel.TotalDistance() >= mTotalDistance);
	}
}
