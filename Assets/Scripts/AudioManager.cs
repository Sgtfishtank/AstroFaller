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
    public Dictionary<string, int[]> mLoadedAudioFixFiles;
    public Dictionary<string, AudioClip[]> mLoadedAudioFiles;

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
        mLoadedAudioFixFiles = new Dictionary<string, int[]>();
        mLoadedAudioFiles = new Dictionary<string, AudioClip[]>();
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
        return GetEvent("Music/" + path, latencyFix, 0);
    }

    public AudioInstanceData GetSoundsEvent(string path, bool latencyFix)
    {
        return GetEvent("Sound/" + path, latencyFix, 0);
    }

    public AudioInstanceData GetMusicEvent(string path, bool latencyFix, int variants)
    {
        return GetEvent("Music/" + path, latencyFix, variants);
	}

    public AudioInstanceData GetSoundsEvent(string path, bool latencyFix, int variants)
    {
        return GetEvent("Sound/" + path, latencyFix, variants);
	}

    public AudioInstanceData GetEvent(string path, bool latencyFix, int variants)
	{
        if (latencyFix)
        {
            // check cached first
            if (mLoadedAudioFixFiles.ContainsKey(path))
            {
                return new AudioInstanceData(mLoadedAudioFixFiles[path]);
            }

#if UNITY_EDITOR
            int acsCount = Resources.LoadAll<AudioClip>(path.Substring(0, path.LastIndexOf('/') + 1)).Length;
            if ((acsCount != variants) && ((acsCount != 1) || (variants != 0)))
            {
                Debug.LogError("Error: illegal variants count: " + path + " varanbtr: " + variants + " vs " + acsCount);
                return null;
            }
#endif

            AudioInstanceData aid = null;
            if (variants == 0)
            {
                int ID = mSoundPool.load(path);
                if (ID == -1)
                {
                    return null;
                }

                aid = new AudioInstanceData(ID);
	        }
            else
            {
                int[] IDs = new int[variants];
                for (int i = 0; i < variants; i++)
                {
                    IDs[i] = mSoundPool.load(path + (i + 1));
                    if (IDs[i] == -1)
                    {
                        return null;
                    }
                }
                aid = new AudioInstanceData(IDs);
            }

            mLoadedAudioFixFiles[path] = aid.mIDs;
            return aid;
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            // check cached first
            if (mLoadedAudioFiles.ContainsKey(path))
            {
                return new AudioInstanceData(mLoadedAudioFiles[path], audioSource);
            }

#if UNITY_EDITOR
            int acsCount = Resources.LoadAll<AudioClip>(path).Length;
            if ((acsCount != variants) && ((acsCount != 1) || (variants != 0)))
            {
                Debug.LogError("Error: illegal variants count:" + path + " varanbtr:" + variants + "vs " + acsCount);
                return null;
            }
#endif

            AudioInstanceData aid = null;
            if (variants == 0)
            {
                AudioClip ac = loadAudioClip(path);
                if (ac == null)
                {
                    return null;
                }

                aid = new AudioInstanceData(ac, audioSource);
            }
            else
            {
                AudioClip[] acs = new AudioClip[variants];
                for (int i = 0; i < variants; i++)
                {
                    acs[i] = loadAudioClip(path + (i + 1));
                    if (acs[i] == null)
                    {
                        return null;
                    }
                }
            
                aid = new AudioInstanceData(acs, audioSource);
            }

            mLoadedAudioFiles[path] = aid.mAudioClips;
            return aid;
        }
    }

    private AudioClip loadAudioClip(string path)
    {
        AudioClip ac = Resources.Load<AudioClip>(path);

#if UNITY_EDITOR
        if (("Assets/Resources/" + path + ".ogg") != AssetDatabase.GetAssetPath(ac))
        {
            print("a: " + "Assets/Resources/" + path);
            print("b: " + AssetDatabase.GetAssetPath(ac));
            Debug.LogError("Error: path not case-sensitive correct:" + path);
            return null;
        }
#endif

        if (ac == null)
        {
            print("error: audio not exist: " + path);
        }

        return ac;
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

        StartAudio(fmodEvent, playOnce, mSoundsLevel * mMasterLevel);
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
            AudioClip ac = fmodEvent.mAudioClips[Random.Range(0, fmodEvent.mAudioClips.Length)];
            if (oneShot)
            {
                fmodEvent.mAudioSource.PlayOneShot(ac, volume);
            }
            else
            {
                fmodEvent.mAudioSource.clip = ac;
                fmodEvent.mAudioSource.volume = volume;
                fmodEvent.mAudioSource.Play();
            }
        }
        else
        {
            float volume2 = fmodEvent.mVolume * volume *  100;
            int playID = fmodEvent.mIDs[Random.Range(0, fmodEvent.mIDs.Length)];
            int streamID = mSoundPool.play(playID, volume2, volume2, 0, fmodEvent.mLoop, 1.0f);
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
