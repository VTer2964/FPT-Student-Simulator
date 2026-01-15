using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TMP_Text dayText;
    public TMP_Text medalText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        dayText.text = $"Day {GameManager.Instance.currentDay} / {GameManager.Instance.maxDay}";
        medalText.text =
            $"G:{GameManager.Instance.gold}  S:{GameManager.Instance.silver}  B:{GameManager.Instance.bronze}";
    }
}
