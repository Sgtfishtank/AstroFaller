using UnityEngine;
using System.Collections;

public class UnlockCriteria : MonoBehaviour 
{
	public Level mCriteriaLevel;

	private LevelBase mLevel;

	// Use this for initialization
	void Start () 
	{
	}

	public void Init()
	{
		mLevel = GetComponent<LevelBase> ();
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public bool CriteriaMet()
	{
		int Distance = GlobalVariables.Instance.DistanceCritera(mLevel.LevelName());

		return (mCriteriaLevel.TotalDistance() >= Distance);
	}
}
