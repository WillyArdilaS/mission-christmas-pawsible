using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHoverHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    
    [SerializeField] private Color highlightColor;
    private Color originalColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color; 
    }

    void OnEnable()
    {
        text.color = originalColor; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor; 
    }  
}