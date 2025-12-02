using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private SplineContainer[] tracks;
    private bool[] trackAvailable;

    // === Properties ===
    public SplineContainer[] Tracks => tracks;

    private void Awake()
    {
        trackAvailable = new bool[tracks.Length];

        for (int i = 0; i < trackAvailable.Length; i++)
        {
            trackAvailable[i] = true;
        }
    }

    public int GetAvailableTrackIndex()
    {
        List<int> freeTracks = new();

        for (int i = 0; i < trackAvailable.Length; i++)
        {
            if (trackAvailable[i]) freeTracks.Add(i);
        }

        if (freeTracks.Count == 0) return -1; // There are no free tracks this frame

        // Choose one randomly from the free ones
        int randomIndex = Random.Range(0, freeTracks.Count);
        return freeTracks[randomIndex];
    }

    public SplineContainer GetTrack(int index)
    {
        return tracks[index];
    }

    public void SetTrackAvailable(int index, bool value)
    {
        trackAvailable[index] = value;
    }
}