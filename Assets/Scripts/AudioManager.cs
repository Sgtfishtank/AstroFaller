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

	public void CopyState(AudioManager mOtherAudioManager)
	{
		// hack set master state
		mOtherAudioManager.mMasterLevel = mMasterLevel;
		mOtherAudioManager.mMuteMaster = mMuteMaster;

		// normal set rest of state
		mOtherAudioManager.MusicLevel(mMusicLevel);
		mOtherAudioManager.SoundsLevel(mSoundsLevel);
		mOtherAudioManager.MuteMusic(mMuteMusic);
		mOtherAudioManager.MuteSounds(mMuteSounds);
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
	
	public float GetMusicLevel()
	{
		return mMusicLevel;
	}
	
	public float GetSoundsLevel()
	{
		return mSoundsLevel;
	}
	
	public float GetMasterLevel()
	{
		return mMasterLevel;
	}

	public bool IsMuteMusic()
	{
		return mMuteMusic;
	}
	
	public bool IsMuteSounds()
	{
		return mMuteSounds;
	}
	
	public bool IsMuteMaster()
	{
		return mMuteMaster;
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
	
	void StartMusic(FMOD.Studio.EventInstance fmodEvent)
	{
		if (mMuteMaster || mMuteMusic)
		{
			return;
		}
		
		fmodEvent.setVolume(mMasterLevel * mSoundsLevel);
		fmodEvent.start ();
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

	// music
	public void PlayMusicOnce(FMOD.Studio.EventInstance fmodEvent)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}
		
		StartMusic(GetEventCopy(fmodEvent));
	}

	public void PlayMusic(FMOD.Studio.EventInstance fmodEvent)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}

		if (!mPlayingMusicEvents.Contains(fmodEvent))
		{
			mPlayingMusicEvents.Add(fmodEvent);
		}

		StartMusic(fmodEvent);
	}

	public void StopMusic(FMOD.Studio.EventInstance fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}

		mPlayingMusicEvents.Remove(fmodEvent);
		fmodEvent.stop (stopMode);
	}

	// sound
	public void PlaySoundOnce(FMOD.Studio.EventInstance fmodEvent)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}

		StartSound(GetEventCopy(fmodEvent));
	}

	public FMOD.Studio.EventInstance GetEventCopy(FMOD.Studio.EventInstance fmodEvent)
	{
		FMOD.Studio.EventDescription ev;
		fmodEvent.getDescription (out ev);

		string path;
		ev.getPath (out path);

		return  FMOD_StudioSystem.instance.GetEvent(path);
	}

	public void PlaySound(FMOD.Studio.EventInstance fmodEvent)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}

		if (!mPlayingSoundEvents.Contains(fmodEvent))
		{
			mPlayingSoundEvents.Add(fmodEvent);
		}

		StartSound(fmodEvent);
	}

	public void StopSound(FMOD.Studio.EventInstance fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		if (fmodEvent == null)
		{
			print("ERROR! Audio missing.");
			return;
		}

		mPlayingSoundEvents.Remove(fmodEvent);
		fmodEvent.stop(stopMode);
	}
}
