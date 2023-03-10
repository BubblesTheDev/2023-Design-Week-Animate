using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class musicHandler : MonoBehaviour
{
    [Header("Music Settings")]
    public List<AudioClip> musicTracks;
    [HideInInspector] public AudioSource musicSource;
    [HideInInspector] public int currentSongIndex;
    public bool loopMusic = true;
    [Range(0f, 1f)]
    public float volumeLevel = 1f;
    
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

        StartCoroutine(playBGMusic());
    }

    private void OnValidate()
    {
        if(musicSource == null) musicSource = GetComponent<AudioSource>();

        musicSource.volume = volumeLevel;
    }

    public IEnumerator playBGMusic()
    {
        if (musicTracks.Count > 0)
        {
            for (currentSongIndex = 0; currentSongIndex < musicTracks.Count - 1; currentSongIndex++)
            {
                musicSource.clip = musicTracks[currentSongIndex];
                musicSource.Play();
                yield return new WaitForSeconds(musicSource.clip.length + 2f);
            }
        }
        else Debug.LogWarning("There are no music tracks set, please set some and restart game");
        if (loopMusic) StartCoroutine(playBGMusic());
    }
}



public struct sound
{
    public string soundName;
    public AudioClip soundClip;

    public static IEnumerator playSound(string soundToPlay, float volumeOfClip, GameObject objToPlaySoundAt)
    {
        //Creating a new Empty object with an audio source on it
        GameObject temp = new GameObject(soundToPlay + " Player");
        temp.transform.parent = GameObject.Find("Audio System").transform;

        AudioSource tempSource = temp.AddComponent<AudioSource>();
        tempSource.volume = volumeOfClip;
        tempSource.clip = GameObject.Find("Audio System").GetComponent<musicHandler>().musicTracks.Where(AudioClip => AudioClip.name == soundToPlay).SingleOrDefault();
        tempSource.loop = false;
        tempSource.Play();
        yield return new WaitForSeconds(tempSource.clip.length + 1f);
        temp = null;
    }
}