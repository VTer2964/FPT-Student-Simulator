using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceSpamMiniGame : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public float gameTime = 5f;
    int score = 0;
    float timer;

    void Start()
    {
        timer = gameTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timeText.text = "Time: " + timer.ToString("0.0");
        scoreText.text = "Score: " + score;

        if (timer > 0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                score++;
            }
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        MedalType result;

        if (score >= 20) result = MedalType.Gold;
        else if (score >= 10) result = MedalType.Silver;
        else result = MedalType.Bronze;

        GameManager.Instance.AddMedal(result);
        SceneManager.LoadScene("Campus_Main");
    }
}