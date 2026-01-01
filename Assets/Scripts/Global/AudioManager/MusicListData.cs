using System;
using UnityEngine;

[Serializable]
public class MusicListData
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] songsArray;
    public AudioClip[] SongsArray => songsArray;
}