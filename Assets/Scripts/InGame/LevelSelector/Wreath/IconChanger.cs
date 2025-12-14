using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IconChanger : MonoBehaviour
{
    // === Sprites ===
    [SerializeField] private Sprite[] iconImages;
    private Dictionary<string, Sprite> iconDictionary;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        iconDictionary = new()
        {
            {"Available", iconImages.FirstOrDefault(icon => icon.name.Contains("Available"))},
            {"Completed", iconImages.FirstOrDefault(icon => icon.name.Contains("Completed"))},
            {"NotAvailable", iconImages.FirstOrDefault(icon => icon.name.Contains("NotAvailable"))}
        };
    }

    public void ChangeIcon(string iconName)
    {
        spriteRenderer.sprite = iconDictionary[iconName];
    }
}
