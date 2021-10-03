using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("AudioManager Instance not found");

            return instance;
        }
    }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource source2D;
    [SerializeField] private List<AudioSource> soundsSources;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    private bool musicFlag = false;

    private float musicVolume = 1f;
    private float soundsVolume = 1f;

    public event EventHandler<OnVolumeChangeEventArgs> OnVolumeChange;
    public class OnVolumeChangeEventArgs : EventArgs
    {
        public float volume;
    }
    public OnVolumeChangeEventArgs OnVolumeChangeArgs;

    #region Clips References

    public enum ClipsTags
    {
        // sounds
        Clic,
        Jump,
        Key,
        Wood,

        // musics

        MainTheme,
        MainMenu,
        GameOver,
        Win,

    }

    [System.Serializable]
    private struct SoundClips
    {
        public string clipName;
        public AudioClip clip;
    }

    [System.Serializable]
    private struct MusicClips
    {
        public string clipName;
        public AudioClip clip;
    }

    [SerializeField] private List<SoundClips> soundClips;
    [SerializeField] private List<MusicClips> musicClips;

    #endregion

    private void Awake()
    {
        instance = this;
        if(!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        if(!PlayerPrefs.HasKey("soundsVolume"))
            PlayerPrefs.SetFloat("soundsVolume", soundsVolume);

        OnVolumeChangeArgs = new OnVolumeChangeEventArgs();

        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        ChangeMusicVolume(musicVolume);
        if(musicSlider != null)
            musicSlider.value = musicVolume;

        soundsVolume = PlayerPrefs.GetFloat("soundsVolume");
        ChangeSoundsVolume(soundsVolume);
        if(soundsSlider != null)
            soundsSlider.value = soundsVolume;

    }

    #region Sounds and music plays

    #region Sounds

    /// <summary>
    /// Returns a audio clip by name
    /// </summary>
    /// <param name="searchedAudio"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(ClipsTags searchedAudio)
    {
        foreach(SoundClips sound in soundClips)
        {
            if(sound.clipName.Equals(searchedAudio.ToString()))
                return sound.clip;
        }

        Debug.LogError(searchedAudio + " not found in Audio Clips.");
        return null;
    }

    /// <summary>
    /// Plays a sound in 2D, the audio source should be located in the AudioManager
    /// </summary>
    /// <param name="searchedSound"></param>
    public void Play2DSound(ClipsTags searchedSound)
    {
        foreach(SoundClips sound in soundClips)
        {
            if(sound.clipName.Equals(searchedSound.ToString()))
            {
                source2D.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError(searchedSound + " not found in Audio Clips.");
    }
    public void Play2DSound(string searchedSound)
    {
        foreach(SoundClips sound in soundClips)
        {
            if(sound.clipName.Equals(searchedSound.ToString()))
            {
                source2D.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError(searchedSound + " not found in Audio Clips.");
    }

    #endregion

    #region Musics

    /// <summary>
    /// Plays a music based of the actual game state. The audio source should be located in the AudioManager
    /// </summary>
    public void PlayMusic()
    {
        string musicToPlay = GameManager.Instance.StateOfGame.ToString();

        if(musicFlag == false)
        {
            musicFlag = true;
            foreach(MusicClips music in musicClips)
            {
                if(music.clipName.Equals(musicToPlay))
                    musicSource.clip = music.clip;
            }
            if(musicSource.clip != null)
                musicSource.Play();
            else
                Debug.LogError("Music not found for " + "\"" + GameManager.Instance.StateOfGame + "\"" + " state of game.");
        }
        else
            Debug.LogError("A music is already playing.");
    }
    /// <summary>
    /// Plays a chosen music. The audio source should be located in the AudioManager
    /// </summary>
    /// <param name="searchedMusic"></param>
    public void PlayMusic(ClipsTags searchedMusic)
    {
        if(musicFlag == false)
        {
            musicFlag = true;
            foreach(MusicClips music in musicClips)
            {
                if(music.clipName.Equals(searchedMusic.ToString()))
                    musicSource.clip = music.clip;
            }
            if(musicSource.clip != null)
                musicSource.Play();
            else
                Debug.LogError(searchedMusic + " not found in Music Clips.");
        }
        else
            Debug.LogError("A music is already playing.");
    }

    /// <summary>
    /// Stops the playing music
    /// </summary>
    public void StopMusic()
    {
        if(musicSource.isPlaying)
        {
            musicSource.Stop();
            musicFlag = false;
        }
    }

    /// <summary>
    /// Stops the playing music, then play one based of the actual game state
    /// </summary>
    public void StopAndReplay()
    {
        StopMusic();
        PlayMusic();
    }

    #endregion

    #endregion

    #region Volumes



    public void ChangeMusicVolume(float val)
    {
        musicSource.volume = val;
        musicVolume = val;
        OnVolumeChange?.Invoke(this, OnVolumeChangeArgs);
    }

    public void ChangeSoundsVolume(float val)
    {
        source2D.volume = val;
        foreach(AudioSource source in soundsSources)
        {
            source.volume = val;
        }
        soundsVolume = val;
        OnVolumeChangeArgs.volume = val;
        OnVolumeChange?.Invoke(this, OnVolumeChangeArgs);
    }

    public void MuteIGSounds(bool mute)
    {
        if(mute)
            OnVolumeChangeArgs.volume = 0;
        else
            OnVolumeChangeArgs.volume = soundsVolume;

        OnVolumeChange?.Invoke(this, OnVolumeChangeArgs);
    }

    #endregion

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("soundsVolume", soundsVolume);
    }
}
