using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    public string targetScene; // Chỉ cần 1 biến này

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Pause 3D + Unlock cursor TRƯỚC khi load
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
        }
    }
}