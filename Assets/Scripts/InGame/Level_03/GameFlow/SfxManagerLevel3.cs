using UnityEngine;

public class SfxManagerLevel3 : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private LightManager lightManager;
    [SerializeField] private SequenceManager sequenceManager;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GlobalGameManager.instance.AudioManager;

        lightManager.ActiveLightsUpdated += _ => PlayLightSwitchSfx();
        sequenceManager.SequenceMatched += PlaySequenceCompleteSfx;
    }

    void OnDestroy()
    {
        lightManager.ActiveLightsUpdated -= _ => PlayLightSwitchSfx();
        sequenceManager.SequenceMatched -= PlaySequenceCompleteSfx;
    }

    private void PlayLightSwitchSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["LightSwitch"], audioManager.LightSwitchVol);
    }

    private void PlaySequenceCompleteSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["SequenceComplete"], audioManager.SequenceCompleteVol);
    }
}