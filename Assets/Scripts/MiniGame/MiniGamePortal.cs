using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    public string sceneName = "MG_SpaceSpam";

    void OnTriggerStay(Collider other)
    {
        // 1. Kiểm tra va chạm
        Debug.Log("Đang chạm vào: " + other.name);

        // 2. Kiểm tra Tag
        if (other.CompareTag("Player"))
        {
            // 3. Kiểm tra phím bấm
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Đã bấm E! Đang chuyển sang: " + sceneName);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                // Dòng này để test xem Unity có nhận phím không (chỉ hiện khi đè phím khác)
                if (Input.anyKeyDown) Debug.Log("Có bấm phím, nhưng không phải E");
            }
        }
        else
        {
            Debug.Log("Sai Tag! Tag của object này là: " + other.tag);
        }
    }
}