using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDayUI : MonoBehaviour
{
    public EndDayPortal portal; // tham chiếu tới EndDayPortal

    public void OnYesButton()
    {
        // người chơi xác nhận kết thúc ngày
        Time.timeScale = 1f;

        GameManager.Instance.NextDay();
        SceneManager.LoadScene("Campus_Main");
    }

    public void OnNoButton()
    {
        // hủy, không kết thúc ngày
        if (portal != null)
        {
            portal.CloseEndDayPanel();
        }
        else
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
