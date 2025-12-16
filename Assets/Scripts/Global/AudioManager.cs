using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// === Structures ===
[Serializable]
public struct MusicList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] songsArray;
    public AudioClip[] SongsArray => songsArray;
}

[Serializable]
public struct SfxList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sfxArray;
    public AudioClip[] SfxArray => sfxArray;
}

[Serializable]
public struct UISfxList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] uiSfxArray;
    public AudioClip[] UISfxArray => uiSfxArray;
}

// === Class ===
[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    // === Audio source ===
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSfxSource;

    // === Music ===
    [SerializeField] private MusicList[] musicList;
    [HideInInspector] public enum MusicScope { MainMenu, InGame }
    private Dictionary<string, AudioClip> musicDictionary;

    // === SFX ===
    [SerializeField] private SfxList[] sfxList;
    [HideInInspector] public enum SfxScope { Level_01, Level_02, Level_03 }
    private Dictionary<string, AudioClip> sfxDictionary = new();

    // === UI SFX ===
    [SerializeField] private UISfxList[] uiSfxList;
    [HideInInspector] public enum UISfxScope { Button }
    private Dictionary<string, AudioClip> uiSfxDictionary = new();

    // === Volumes ===
    [Header("Music Volumes")]
    [SerializeField, Range(0, 1)] private float mainMenuVol;
    [SerializeField, Range(0, 1)] private float level01Vol;
    [SerializeField, Range(0, 1)] private float level02Vol;
    [SerializeField, Range(0, 1)] private float level03Vol;

    [Header("Level 01 SFX Volumes")]
    [SerializeField, Range(0, 1)] private float hitObstacleVol;
    [SerializeField, Range(0, 1)] private float lapUpdatedVol;
    [SerializeField, Range(0, 1)] private float powerupVol;
    [SerializeField, Range(0, 1)] private float raceStartVol;

    [Header("Level 02 SFX Volumes")]
    [SerializeField, Range(0, 1)] private float coalRecolectedVol;
    [SerializeField, Range(0, 1)] private float fruitRecolectedVol;

    [Header("Level 03 SFX Volumes")]
    [SerializeField, Range(0, 1)] private float lightSwitchVol;
    [SerializeField, Range(0, 1)] private float sequenceCompleteVol;

    [Header("UI SFX Volumes")]
    [SerializeField, Range(0, 1)] private float buttonClickVol;

    // === Properties ===
    public Dictionary<string, AudioClip> SfxDictionary => sfxDictionary;
    public Dictionary<string, AudioClip> UISfxDictionary => uiSfxDictionary;
    public float MainMenuVol => mainMenuVol;
    public float Level01Vol => level01Vol;
    public float Level02Vol => level02Vol;
    public float Level03Vol => level03Vol;
    public float HitObstacleVol => hitObstacleVol;
    public float LapUpdatedVol => lapUpdatedVol;
    public float PowerupVol => powerupVol;
    public float RaceStartVol => raceStartVol;
    public float CoalRecolectedVol => coalRecolectedVol;
    public float FruitRecolectedVol => fruitRecolectedVol;
    public float LightSwitchVol => lightSwitchVol;
    public float SequenceCompleteVol => sequenceCompleteVol;
    public float ButtonClickVol => buttonClickVol;

    // === Initialization Methods ===
    void Awake()
    {
        musicDictionary = new()
        {
            {"MainMenu", FindMusicDictionaryValue("MainMenu")},
            {"Level_01", FindMusicDictionaryValue("Level_01")},
            {"Level_02", FindMusicDictionaryValue("Level_02")},
            {"Level_03", FindMusicDictionaryValue("Level_03")},
        };

        sfxDictionary = new()
        {
            {"HitObstacle", FindSfxDictionaryValue("HitObstacle")},
            {"LapUpdated", FindSfxDictionaryValue("LapUpdated")},
            {"Powerup", FindSfxDictionaryValue("Powerup")},
            {"RaceStart", FindSfxDictionaryValue("RaceStart")},
            {"CoalRecolected", FindSfxDictionaryValue("CoalRecolected")},
            {"FruitRecolected", FindSfxDictionaryValue("FruitRecolected")},
            {"LightSwitch", FindSfxDictionaryValue("LightSwitch")},
            {"SequenceComplete", FindSfxDictionaryValue("SequenceComplete")},
        };

        uiSfxDictionary = new()
        {
            {"Click", FindUISfxDictionaryValue("Click")}
        };
    }

    private AudioClip FindMusicDictionaryValue(string key)
    {
        int musicListIndex = Array.FindIndex(musicList, music => music.SongsArray.FirstOrDefault(song => song.name == key));
        AudioClip musicClip = Array.Find(musicList[musicListIndex].SongsArray, song => song.name == key);

        return musicClip;
    }

    private AudioClip FindSfxDictionaryValue(string key)
    {
        int sfxListIndex = Array.FindIndex(sfxList, sfx => sfx.SfxArray.FirstOrDefault(sfx => sfx.name == key));
        AudioClip sfxClip = Array.Find(sfxList[sfxListIndex].SfxArray, sfx => sfx.name == key);

        return sfxClip;
    }

    private AudioClip FindUISfxDictionaryValue(string key)
    {
        int uiSfxListIndex = Array.FindIndex(uiSfxList, uiSfx => uiSfx.UISfxArray.FirstOrDefault(uiSfx => uiSfx.name == key));
        AudioClip uiSfxClip = Array.Find(uiSfxList[uiSfxListIndex].UISfxArray, uiSfx => uiSfx.name == key);

        return uiSfxClip;
    }

    // === Music methods ===
    private void PlayMusic(AudioClip clip, float volume = 1)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void UnPauseMusic()
    {
        musicSource.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public AudioClip GetMusicClip()
    {
        return musicSource.clip;
    }

    public void SelectSceneMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                if (GetMusicClip() == null || GetMusicClip().name != "MainMenu") // To avoid restarting the music if it is already playing
                {
                    PlayMusic(musicDictionary["MainMenu"], mainMenuVol);
                }
                break;
            case "LevelSelector":
                StopMusic();
                break;
            case "Level_01":
                PlayMusic(musicDictionary["Level_01"], level01Vol);
                break;
            case "Level_02":
                PlayMusic(musicDictionary["Level_02"], level02Vol);
                break;
            case "Level_03":
                PlayMusic(musicDictionary["Level_03"], level03Vol);
                break;
        }
    }

    // === SFX methods ===
    public void PlaySFX(AudioClip clip, float volume = 1)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // === UI SFX methods ===
    public void PlayUISFX(AudioClip clip, float volume = 1)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // === Synchronization of arrays with enums (only in the editor) ===
#if UNITY_EDITOR
    void OnEnable()
    {
        // For Music
        string[] musicNames = Enum.GetNames(typeof(MusicScope)); // Get the names of all values in the MusicType enum
        Array.Resize(ref musicList, musicNames.Length);

        for (int i = 0; i < musicList.Length; i++)
        {
            musicList[i].name = musicNames[i];
        }

        // For SFX
        string[] sfxNames = Enum.GetNames(typeof(SfxScope)); // Get the names of all values in the SfxType enum
        Array.Resize(ref sfxList, sfxNames.Length);

        for (int i = 0; i < sfxList.Length; i++)
        {
            sfxList[i].name = sfxNames[i];
        }

        // For UI SFX
        string[] uiSfxNames = Enum.GetNames(typeof(UISfxScope)); // Get the names of all values in the SfxType enum
        Array.Resize(ref uiSfxList, uiSfxNames.Length);

        for (int i = 0; i < uiSfxList.Length; i++)
        {
            uiSfxList[i].name = uiSfxNames[i];
        }
    }
#endif
}