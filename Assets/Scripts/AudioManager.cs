using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public AudioInstanceData GetMusicEvent(string path, bool latencyFix)
    {
        return GetEvent("Music/" + path, latencyFix);
		//return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent("event:/Music/" + path));
	}
	
	public AudioInstanceData GetSoundsEvent(string path, bool latencyFix)
    {
        return GetEvent("Sound/" + path, latencyFix);
		//return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent("event:/Sounds/" + path));
	}
	
    public AudioInstanceData GetEvent(string path, bool latencyFix)
	{
        if (latencyFix)
        {
            return new AudioInstanceData(mSoundPool.load(path));
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = Resources.Load<AudioClip>(path);

#if UNITY_EDITOR
            if (("Assets/Resources/" + path + ".ogg") != AssetDatabase.GetAssetPath(audioSource.clip))
            {
                print("a: " + "Assets/Resources/" + path);
                print("b: " + AssetDatabase.GetAssetPath(audioSource.clip));
                Debug.LogError("Error: path not case-sensitive correct:" + path);
            }
#endif

            if (audioSource.clip == null)
            {
                print("error: audio not exist: " + path);
                return null;
            }

            return new AudioInstanceData(audioSource);
        }
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
                StopAudio(mPlayingSoundEvents[i]);
                //mPlayingSoundEvents[i].mEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
            {
                StartAudio(mPlayingSoundEvents[i], false, mSoundsLevel * mMasterLevel);
                //mPlayingSoundEvents[i].mEvent.start();
			}
		}
	}

	void UpdateMuteMusic()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			if (mMuteMaster || mMuteMusic) 
			{
                StopAudio(mPlayingMusicEvents[i]);
                //mPlayingMusicEvents[i].mEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
            {
                StartAudio(mPlayingMusicEvents[i], false, mMusicLevel * mMasterLevel);
                //mPlayingMusicEvents[i].mEvent.start();
			}
		}
	}

	void UpdateSoundsLevel()
	{
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
            SetAudioVolume(mPlayingSoundEvents[i], mSoundsLevel * mMasterLevel);
			//mPlayingSoundEvents[i].setVolume(mSoundsLevel * mMasterLevel);
		}
	}

	void UpdateMusicLevel()
	{
		for (int i = 0; i < mPlayingMusicEvents.Count; i++)
        {
            SetAudioVolume(mPlayingMusicEvents[i], mMusicLevel * mMasterLevel);
			//mPlayingMusicEvents[i].mEvent.setVolume(mMusicLevel * mMasterLevel);
		}
	}
	
	// music
	public void PlayMusicOnce(AudioInstanceData fmodEvent)
    {
        PlayMusic(fmodEvent, true);
	}

	public void PlayMusic(AudioInstanceData fmodEvent)
	{
        PlayMusic(fmodEvent, false);
	}

    public void PlayMusic(AudioInstanceData fmodEvent, bool playOnce)
    {
        if (fmodEvent == null)
        {
            Debug.LogError("ERROR! Audio missing.");
            return;
        }

        if (!playOnce)
        {
            if (!mPlayingMusicEvents.Contains(fmodEvent))
            {
                mPlayingMusicEvents.Add(fmodEvent);
            }

            if (mDebug)
            {
                printMusic();
            }
        }

        if (mMuteMaster || mMuteMusic)
        {
            return;
        }

        StartAudio(fmodEvent, false, mMusicLevel * mMasterLevel);
    }

	void printSound()
	{
		string strOutput = "Playing sounds " + mPlayingSoundEvents.Count + " ";
		for (int i = 0; i < mPlayingSoundEvents.Count; i++) 
		{
			//strOutput += GetFmodPath(mPlayingSoundEvents[i]) + " ";
		}
		print(strOutput);
	}
	
	void printMusic()
	{
		string strOutput = "Playing music " + mPlayingMusicEvents.Count + " ";
		for (int i = 0; i < mPlayingMusicEvents.Count; i++) 
		{
			//strOutput += GetFmodPath(mPlayingMusicEvents[i]) + " ";
		}
		print(strOutput);
	}

	public void StopMusic(AudioInstanceData fmodEvent)
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

        StopAudio(fmodEvent);
	}

    // sound
	public void PlaySound(AudioInstanceData fmodEvent)
    {
        PlaySound(fmodEvent, false);
	}

    public void PlaySoundOnce(AudioInstanceData fmodEvent)
    {
        PlaySound(fmodEvent, true);
    }

    public void PlaySound(AudioInstanceData fmodEvent, bool playOnce)
    {
        if (fmodEvent == null)
        {
            Debug.LogError("ERROR! Audio missing.");
            return;
        }

        if (!playOnce)
        {
		    if (!mPlayingSoundEvents.Contains(fmodEvent))
		    {
			    mPlayingSoundEvents.Add(fmodEvent);
            }

            if (mDebug)
            {
                printSound();
            }
        }

        if (mMuteMaster || mMuteSounds)
        {
            return;
        }

        StartAudio(fmodEvent, true, mSoundsLevel * mMasterLevel);
    }

	public void StopSound(AudioInstanceData fmodEvent)
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

        StopAudio(fmodEvent);
	}

    void StopAudio(AudioInstanceData fmodEvent)
    {
        if (fmodEvent.mAudioSource != null)
        {
            fmodEvent.mAudioSource.Stop();
        }
        else
        {
            mSoundPool.stop(fmodEvent.mStreamID);
            fmodEvent.mStreamID = 0;
        }
    }

    void StartAudio(AudioInstanceData fmodEvent, bool oneShot, float volume)
    {
        if (fmodEvent.mAudioSource != null)
        {
            if (oneShot)
            {
                fmodEvent.mAudioSource.PlayOneShot(fmodEvent.mAudioSource.clip, volume);
            }
            else
            {
                fmodEvent.mAudioSource.volume = volume;
                fmodEvent.mAudioSource.Play();
            }
        }
        else
        {
            if (fmodEvent.mID == -1)
            {
                return;
            }

            float volume2 = fmodEvent.mVolume * volume *  100;
            int streamID = mSoundPool.play(fmodEvent.mID, volume2, volume2, 0, fmodEvent.mLoop, 1.0f);
            if (!oneShot)
            {
                fmodEvent.mStreamID = streamID;
            }
        }
    }

    private void SetAudioVolume(AudioInstanceData audioInstanceData, float p)
    {
        if (audioInstanceData.mAudioSource != null)
        {
            audioInstanceData.mAudioSource.volume = p;
        }
        else
        {
            if (audioInstanceData.mStreamID == 0)
            {
                return;
            }
            mSoundPool.setVolume(audioInstanceData.mStreamID, p * 100, p * 100);
        }
    }

    /*public string GetFmodPath(AudioInstanceData fmodEvent)
    {
        FMOD.Studio.EventDescription ed;
        string path;
        fmodEvent.getDescription(out ed);
        ed.getPath(out path);
        return path;
        return null;
    }*/

    /*public AudioInstanceData GetEventCopy(AudioInstanceData fmodEvent)
    {
        FMOD.Studio.EventDescription ev;
        //fmodEvent.getDescription (out ev);

        string path = "";
        //ev.getPath (out path);

        return new AudioInstanceData(FMOD_StudioSystem.instance.GetEvent(path));
    }*/
}
