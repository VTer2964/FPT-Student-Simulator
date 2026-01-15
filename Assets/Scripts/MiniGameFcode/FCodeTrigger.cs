using UnityEngine;
using UnityEngine.SceneManagement;

public class FCodeTrigger : MonoBehaviour
{
    public string sceneName = "MG_FCode";

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
