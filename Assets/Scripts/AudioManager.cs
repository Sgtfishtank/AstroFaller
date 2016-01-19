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
	public bool mDebug = false;
	public SoundPool mSoundPool;

	public List<AudioInstanceData> mPlayingSoundEvents;
	public List<AudioInstanceData> mPlayingMusicEvents;

	private static AudioManager instance = null;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Singleton<AudioManager>.CreateInstance("Prefab/Audio Manager");
				GameObject.DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	void Awake() 
	{
		mPlayingSoundEvents = new List<AudioInstanceData> ();
		mPlayingMusicEvents = new List<AudioInstanceData> ();
		mSoundPool = GetComponent<SoundPool>();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

    public AudioInstanceData GetEvent(string path)
	{
		return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent("event:/" + path));
    }

    public AudioInstanceData GetMusicEvent(string path)
    {
		return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent("event:/Music/" + path));
	}
	
	public AudioInstanceData GetSoundsEvent(string path)
    {
		return new AudioInstanceData(mSoundPool.load("Sound/" + path), -1);
		//return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent("event:/Sounds/" + path));
	}
	
	public void CopyState(AudioManager mOtherAudioManager)
	{
		// set state to other state
		mOtherAudioManager.MuteMaster(mMuteMusic);
		mOtherAudioManager.MasterLevel(mMasterLevel);
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
				mPlayingSoundEvents[i].mEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				mPlayingSoundEvents[i].mEvent.start();
			}
		}
	}
	
	void UpdateMuteMusic()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			if (mMuteMaster || mMuteMusic) 
			{
				mPlayingMusicEvents[i].mEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				mPlayingMusicEvents[i].mEvent.start();
			}
		}
	}

	void UpdateSoundsLevel()
	{
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
			mPlayingSoundEvents[i].mEvent.setVolume(mSoundsLevel * mMasterLevel);
		}
	}
	
	void UpdateMusicLevel()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			mPlayingMusicEvents[i].mEvent.setVolume(mMusicLevel * mMasterLevel);
		}
	}
	
	void StartMusic(AudioInstanceData fmodEvent)
	{
		if (mMuteMaster || mMuteMusic)
		{
			return;
		}
		
		fmodEvent.mEvent.setVolume(mMasterLevel * mSoundsLevel);
		fmodEvent.mEvent.start ();
	}
	
	void StartSound(AudioInstanceData fmodEvent)
	{
		if (mMuteMaster || mMuteSounds)
		{
			return;
		}
		
		fmodEvent.mEvent.setVolume(mMasterLevel * mSoundsLevel);
		fmodEvent.mEvent.start();
	}

	// music
	public void PlayMusicOnce(AudioInstanceData fmodEvent)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		if (mDebug) 
		{
			print ("playing music once " + GetFmodPath (fmodEvent));
		}
		StartMusic(GetEventCopy(fmodEvent));
	}

	public void PlayMusic(AudioInstanceData fmodEvent)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		if (!mPlayingMusicEvents.Contains(fmodEvent))
		{
			mPlayingMusicEvents.Add(fmodEvent);
			if (mDebug) 
			{
				printMusic();
			}
		}

		StartMusic(fmodEvent);
	}

	void printSound()
	{
		string strOutput = "Playing sounds " + mPlayingSoundEvents.Count + " ";
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
			strOutput += GetFmodPath(mPlayingSoundEvents[i]) + " ";
		}
		print(strOutput);
	}
	
	void printMusic()
	{
		string strOutput = "Playing music " + mPlayingMusicEvents.Count + " ";
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			strOutput += GetFmodPath(mPlayingMusicEvents[i]) + " ";
		}
		print(strOutput);
	}

	public string GetFmodPath(AudioInstanceData fmodEvent)
	{
		FMOD.Studio.EventDescription ed;
		string path;
		fmodEvent.mEvent.getDescription(out ed);
		ed.getPath(out path);
		return path;
	}

	public void StopMusic(AudioInstanceData fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		if (mPlayingMusicEvents.Remove(fmodEvent) && mDebug) 
		{
			printMusic();
		}

		fmodEvent.mEvent.stop (stopMode);
	}

	// sound
	public void PlaySoundOnce(AudioInstanceData fmodEvent)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		print (fmodEvent.mID);
		print (fmodEvent.mVolume);
		mSoundPool.playOnce(fmodEvent.mID, fmodEvent.mVolume, fmodEvent.mVolume, 0, 0, 1);
		//fmodEvent.mStreamID = new SoundPool ().play (fmodEvent.mID, fmodEvent.mVolume, fmodEvent.mVolume, 0, 0, 1);

		//StartSound(GetEventCopy(fmodEvent));
		if (mDebug) 
		{
			print("playing sound once " + GetFmodPath(fmodEvent));
		}
	}

	public AudioInstanceData GetEventCopy(AudioInstanceData fmodEvent)
	{
		FMOD.Studio.EventDescription ev;
		fmodEvent.mEvent.getDescription (out ev);

		string path;
		ev.getPath (out path);

		return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent(path));
	}

	public void PlaySound(AudioInstanceData fmodEvent)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		if (!mPlayingSoundEvents.Contains(fmodEvent))
		{
			mPlayingSoundEvents.Add(fmodEvent);
			if (mDebug) 
			{
				printSound();
			}
		}

		StartSound(fmodEvent);
	}

	public void StopSound(AudioInstanceData fmodEvent, FMOD.Studio.STOP_MODE stopMode)
	{
		if (fmodEvent == null)
		{
			Debug.LogError("ERROR! Audio missing.");
			return;
		}

		if (mPlayingSoundEvents.Remove (fmodEvent) && mDebug) 
		{
			printSound();
		}
		fmodEvent.mEvent.stop(stopMode);
	}
}
