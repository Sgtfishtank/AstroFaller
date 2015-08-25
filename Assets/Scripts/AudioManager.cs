using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	public float mMasterLevel = 1;
	public float mMusicLevel = 1;
	public float mSoundsLevel = 1;
	public bool mMuteMaster = false;
	public bool mMuteSounds = false;
	public bool mMuteMusic = false;

	public List<FMOD.Studio.EventInstance> mPlayingSoundEvents;
	public List<FMOD.Studio.EventInstance> mPlayingMusicEvents;

	private static AudioManager instance = null;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Audio Manager");
				instance = thisObject.GetComponent<AudioManager>();
			}
			return instance;
		}
	}

	void Awake() 
	{
		mPlayingSoundEvents = new List<FMOD.Studio.EventInstance> ();
		mPlayingMusicEvents = new List<FMOD.Studio.EventInstance> ();
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void MusicLevel(float level)
	{
		mMusicLevel = level;
		UpdateMusicLevel();
	}
	
	public void SoundsLevel(float level)
	{
		mSoundsLevel = level;
		UpdateSoundsLevel();
	}
	
	public void MasterLevel(float level)
	{
		mMasterLevel = level;
		UpdateSoundsLevel();
		UpdateMusicLevel();
	}

	public void MuteMusic(bool mute)
	{
		bool updateMusic = ((mMuteMaster || mMuteMusic) != mute);

		mMuteMusic = mute;

		if (updateMusic)
		{
			UpdateMuteMusic();
		}
	}
	
	public void MuteSounds(bool mute)
	{
		bool updateSounds = ((mMuteMaster || mMuteSounds) != mute);

		mMuteSounds = mute;

		if (updateSounds)
		{
			UpdateMuteSounds();
		}
	}
	
	public void MuteMaster(bool mute)
	{
		bool updateMusic = ((mMuteMaster || mMuteMusic) != mute);
		bool updateSounds = ((mMuteMaster || mMuteSounds) != mute);

		mMuteMaster = mute;

		if (updateMusic)
		{
			UpdateMuteMusic();
		}
		if (updateSounds)
		{
			UpdateMuteSounds();
		}
	}
	
	void UpdateMuteSounds()
	{
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
			if (mMuteMaster || mMuteSounds) 
			{
				mPlayingSoundEvents[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				mPlayingSoundEvents[i].start();
			}
		}
	}
	
	void UpdateMuteMusic()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			if (mMuteMaster || mMuteMusic) 
			{
				mPlayingMusicEvents[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				mPlayingMusicEvents[i].start();
			}
		}
	}

	void UpdateSoundsLevel()
	{
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
			mPlayingSoundEvents[i].setVolume(mSoundsLevel * mMasterLevel);
		}
	}
	
	void UpdateMusicLevel()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			mPlayingMusicEvents[i].setVolume(mMusicLevel * mMasterLevel);
		}
	}

	public void PlayMusicOnce(FMOD.Studio.EventInstance fmodEvent)
	{
		StartMusic(fmodEvent);
	}

	public void PlayMusic(FMOD.Studio.EventInstance fmodEvent)
	{
		if (!mPlayingMusicEvents.Contains(fmodEvent))
		{
			mPlayingMusicEvents.Add(fmodEvent);
		}

		StartMusic(fmodEvent);
	}
	
	void StartMusic(FMOD.Studio.EventInstance fmodEvent)
	{
		if (mMuteMaster || mMuteMusic)
		{
			return;
		}

		fmodEvent.setVolume(mMasterLevel * mSoundsLevel);
		fmodEvent.start ();
	}

	public void StopMusic(FMOD.Studio.EventInstance fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		mPlayingMusicEvents.Remove(fmodEvent);
		fmodEvent.stop (stopMode);
	}

	public void PlaySoundOnce(FMOD.Studio.EventInstance fmodEvent)
	{
		StartSound(fmodEvent);
	}
	
	public void PlaySound(FMOD.Studio.EventInstance fmodEvent)
	{
		if (!mPlayingSoundEvents.Contains(fmodEvent))
		{
			mPlayingSoundEvents.Add(fmodEvent);
		}

		StartSound(fmodEvent);
	}

	void StartSound(FMOD.Studio.EventInstance fmodEvent)
	{
		if (mMuteMaster || mMuteSounds)
		{
			return;
		}

		fmodEvent.setVolume(mMasterLevel * mSoundsLevel);
		fmodEvent.start();
	}

	public void StopSound(FMOD.Studio.EventInstance fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		mPlayingSoundEvents.Remove(fmodEvent);
		fmodEvent.stop(stopMode);
	}
}
