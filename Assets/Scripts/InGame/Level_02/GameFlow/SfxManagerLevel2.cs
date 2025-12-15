using UnityEngine;

public class SfxManagerLevel2 : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private CollisionManagerLevel2 collisionManager;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameManager.instance.AudioManager;

        collisionManager.CoalRecolected += PlayCoalRecolectedSfx;
        collisionManager.FruitRecolected += _ => PlayFruitRecolectedSfx();
    }

    void OnDestroy()
    {
        collisionManager.CoalRecolected -= PlayCoalRecolectedSfx;
        collisionManager.FruitRecolected -= _ => PlayFruitRecolectedSfx();
    }

    private void PlayCoalRecolectedSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["CoalRecolected"], audioManager.CoalRecolectedVol);
    }

    private void PlayFruitRecolectedSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["FruitRecolected"], audioManager.FruitRecolectedVol);
    }
}
