using UnityEngine;

public class EndDayPortal : MonoBehaviour
{
    public GameObject endDayPanel;   // gán Panel ở Inspector
    bool playerInRange = false;

    void Update()
    {
        // Khi đứng trong cổng và bấm E thì mở panel
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenEndDayPanel();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Có thể hiện 1 text nhỏ: "Nhấn E để kết thúc ngày"
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void OpenEndDayPanel()
    {
        if (endDayPanel != null)
        {
            endDayPanel.SetActive(true);
            Time.timeScale = 0f;

            // mở khóa chuột để bấm nút
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // tắt điều khiển nhìn
            MouseLook ml = Camera.main.GetComponent<MouseLook>();
            if (ml != null) ml.canLook = false;
        }
    }

    public void CloseEndDayPanel()
    {
        if (endDayPanel != null)
        {
            endDayPanel.SetActive(false);
            Time.timeScale = 1f;

            // khóa chuột lại
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            MouseLook ml = Camera.main.GetComponent<MouseLook>();
            if (ml != null) ml.canLook = true;
        }
    }

}
