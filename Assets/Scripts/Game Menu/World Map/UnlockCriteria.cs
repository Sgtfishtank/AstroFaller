using UnityEngine;
using System.Collections;

public class UnlockCriteria : MonoBehaviour 
{
	public Level mCriteriaLevel;

	private LevelBase mLevel;

	void Awake()
	{
		mLevel = GetComponent<LevelBase> ();
	}

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
		int Distance = GlobalVariables.Instance.DistanceCritera(mLevel.LevelName());

		return (mCriteriaLevel.TotalDistance() >= Distance);
	}
}
