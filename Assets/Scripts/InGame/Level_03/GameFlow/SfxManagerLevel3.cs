using UnityEngine;

public class SfxManagerLevel3 : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private LightManager lightManager;
    [SerializeField] private TreeAnimator treeAnimator;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameManager.instance.AudioManager;

        lightManager.ActiveLightsUpdated += _ => PlayLightSwitchSfx();
        treeAnimator.LightedTree += PlaySequenceCompleteSfx;
    }

    void OnDestroy()
    {
        lightManager.ActiveLightsUpdated -= _ => PlayLightSwitchSfx();
        treeAnimator.LightedTree -= PlaySequenceCompleteSfx;
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