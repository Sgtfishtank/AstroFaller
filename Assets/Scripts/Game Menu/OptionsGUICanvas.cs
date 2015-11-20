using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsGUICanvas : MonoBehaviour 
{
	private GameObject mOptionButtons;
	private GameObject mMasterButtons;
	private GameObject mSoundButtons;
	private GameObject mMusicButtons;

	private bool mShowButtons;
	private Button[] mButtons;
	private ButtonPress[] mButtonPresss;
	
	void Awake () 
	{
		//assign all option buttons
		mOptionButtons = transform.Find ("OptionButtons").gameObject;
		mMasterButtons = mOptionButtons.transform.Find("Master").gameObject;
		mSoundButtons = mOptionButtons.transform.Find("Sounds").gameObject;
		mMusicButtons = mOptionButtons.transform.Find("Music").gameObject;
		mOptionButtons.SetActive (true);

		mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
		mButtons = GetComponentsInChildren<Button>(true);
	}
	
	// Use this for initialization
	void Start () 
	{
		UpdateOptions ();
		
		for (int i = 0; i < mButtonPresss.Length; i++) 
		{
			mButtonPresss[i].Init();	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public Button[] GetButtons ()
	{
		return mButtons;
	}
	
	// toggle menu buttons
	public void Deselect()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons ();
		MainGameMenu.Instance.UpdateMenusAndButtons ();
	}

	// buttons
	public GameObject OptionButtons()
	{
		return mOptionButtons;
	}
	
	// options
	public void MuteMaster()
	{
		AudioManager.Instance.MuteMaster(mMasterButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}
	
	public void MuteSounds()
	{
		AudioManager.Instance.MuteSounds(mSoundButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}
	
	public void MuteMusic()
	{
		AudioManager.Instance.MuteMusic(mMusicButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}
	
	public void MasterLevel()
	{
		AudioManager.Instance.MasterLevel(mMasterButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void SoundsLevel()
	{
		AudioManager.Instance.SoundsLevel(mSoundButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void MusicLevel()
	{
		AudioManager.Instance.MusicLevel(mMusicButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void AcceptOptions()
	{
		MainGameMenu.Instance.ShowOptions (false, false);
	}
	
	public void CancelOptions()
	{
		MainGameMenu.Instance.ShowOptions (false, true);
	}
	
	//
	public void UpdateOptions()
	{
		mMusicButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteMusic();
		mSoundButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteSounds();
		mMasterButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteMaster();
		mMusicButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetMusicLevel();
		mSoundButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetSoundsLevel();
		mMasterButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetMasterLevel();
	}
}
