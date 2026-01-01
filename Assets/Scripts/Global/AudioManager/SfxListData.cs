using System;
using UnityEngine;

[Serializable]
public class SfxListData
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sfxArray;
    public AudioClip[] SfxArray => sfxArray;
}