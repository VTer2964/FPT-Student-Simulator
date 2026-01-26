using UnityEngine;
using TMPro;
using System.Collections; // Cần dùng để làm Animation

public class SudokuCell : MonoBehaviour
{
    public int correctNumber;
    public int currentNumber = 0;
    public bool isLocked = false;

    [Header("Visuals")]
    public TextMeshPro textDisplay;
    public Renderer cellRenderer; // Kéo chính cái Cube vào đây để đổi màu nền

    [Header("Settings")]
    public Color lockedColor = new Color(0.7f, 0.7f, 0.7f); // Màu xám (đề bài)
    public Color normalColor = Color.white;                 // Màu trắng (ô thường)
    public Color selectColor = new Color(0.6f, 1f, 0.6f);   // Màu xanh (khi được chọn - Optional)
    public Color numberColorLocked = Color.black;
    public Color numberColorPlayer = new Color(0.2f, 0.4f, 0.8f); // Xanh đậm cho số người chơi điền

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        if (cellRenderer == null) cellRenderer = GetComponent<Renderer>();
    }

    public void Setup(int number, bool locked, int answer)
    {
        currentNumber = number;
        isLocked = locked;
        correctNumber = answer;
        UpdateVisual();
    }

    // Hàm tương tác chính
    public void Interact()
    {
        if (isLocked) return; // Nếu bị khóa thì thôi

        // Tăng số: 0 (trống) -> 1 -> ... -> 9 -> 0
        currentNumber++;
        if (currentNumber > 9) currentNumber = 0;

        UpdateVisual();

        // Kích hoạt hiệu ứng nảy
        StopAllCoroutines();
        StartCoroutine(PopAnimation());
    }

    void UpdateVisual()
    {
        // 1. Cập nhật chữ số
        if (currentNumber == 0) textDisplay.text = "";
        else textDisplay.text = currentNumber.ToString();

        // 2. Cập nhật màu sắc
        if (isLocked)
        {
            textDisplay.color = numberColorLocked;
            cellRenderer.material.color = lockedColor;
        }
        else
        {
            textDisplay.color = numberColorPlayer;
            cellRenderer.material.color = normalColor;
        }
    }

    // Hiệu ứng "Nảy" (Scale up -> Scale down)
    IEnumerator PopAnimation()
    {
        // Phóng to lên 1.2 lần trong 0.1 giây
        float timer = 0;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.2f, timer / 0.1f);
            yield return null;
        }

        // Thu nhỏ về bình thường trong 0.1 giây
        timer = 0;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale * 1.2f, originalScale, timer / 0.1f);
            yield return null;
        }

        transform.localScale = originalScale; // Đảm bảo về đúng kích thước chuẩn
    }
}