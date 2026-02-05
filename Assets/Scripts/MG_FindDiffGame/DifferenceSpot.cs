using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifferenceSpotUI : MonoBehaviour, IPointerClickHandler
{
    public bool isFound = false;
    public UnityEvent onFound;

    private Image highlight;

    void Start()
    {
 
        GameObject hlObj = new GameObject("Highlight");
        hlObj.transform.SetParent(transform, false);

        highlight = hlObj.AddComponent<Image>();
        highlight.color = new Color(0, 1, 0, 0.3f);
        highlight.enabled = false;

        // ← THÊM ĐOẠN NÀY: Fit theo Spot size
        RectTransform hlRect = hlObj.GetComponent<RectTransform>();
        hlRect.anchorMin = Vector2.zero;
        hlRect.anchorMax = Vector2.one;
        hlRect.offsetMin = Vector2.zero;
        hlRect.offsetMax = Vector2.zero;
        hlRect.sizeDelta = Vector2.zero;

        highlight.material = new Material(Shader.Find("UI/Unlit/Transparent"));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFound) return;
        isFound = true;
        highlight.enabled = true;
        onFound.Invoke();
    }
}
