using System;
using UnityEngine;

[Serializable]
public class UISfxListData
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] uiSfxArray;
    public AudioClip[] UISfxArray => uiSfxArray;
}