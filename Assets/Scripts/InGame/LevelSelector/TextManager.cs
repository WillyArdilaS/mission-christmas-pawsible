using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject winText;

    void Awake()
    {
        if (LevelSelectorManager.instance.NextLevelState != LevelSelectorManager.NextLevel.Finished)
        {
            continueText.SetActive(true);
            winText.SetActive(false);
        }
        else
        {
            continueText.SetActive(false);
            winText.SetActive(true);
        }
    }
}