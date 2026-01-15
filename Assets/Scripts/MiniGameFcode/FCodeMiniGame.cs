using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FCodeMiniGame : MonoBehaviour
{
    public TMP_Text targetText;
    public TMP_Text inputText;
    public TMP_Text timerText;
    public TMP_Text correctCountText;

    public float totalTime = 30f;
    public int minLength = 5;
    public int maxLength = 8;

    float timer;
    string currentTarget;
    StringBuilder currentInput = new StringBuilder();
    int correctCount = 0;
    bool gameOver = false;

    void Start()
    {
        timer = totalTime;
        GenerateNewTarget();
    }

    void Update()
    {
        if (gameOver) return;

        // Đếm giờ
        timer -= Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("0.0");
        correctCountText.text = "Correct: " + correctCount;

        if (timer <= 0f)
        {
            timer = 0f;
            EndGame();
            return;
        }

        // Nhận input phím chữ
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                char upper = char.ToUpper(c);
                currentInput.Append(upper);
                inputText.text = currentInput.ToString();

                // nếu đủ độ dài thì kiểm tra
                if (currentInput.Length == currentTarget.Length)
                {
                    CheckCurrentInput();
                }
            }
            // Backspace xoá 1 ký tự
            else if (c == '\b' && currentInput.Length > 0)
            {
                currentInput.Remove(currentInput.Length - 1, 1);
                inputText.text = currentInput.ToString();
            }
        }
    }

    void GenerateNewTarget()
    {
        int len = Random.Range(minLength, maxLength + 1);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < len; i++)
        {
            char ch = (char)Random.Range(65, 91); // A-Z
            sb.Append(ch);
        }
        currentTarget = sb.ToString();
        targetText.text = currentTarget;
        currentInput.Clear();
        inputText.text = "";
    }

    void CheckCurrentInput()
    {
        if (currentInput.ToString() == currentTarget)
        {
            correctCount++;
        }

        GenerateNewTarget();
    }

    void EndGame()
    {
        gameOver = true;

        MedalType result;
        if (correctCount >= 8) result = MedalType.Gold;
        else if (correctCount >= 4) result = MedalType.Silver;
        else result = MedalType.Bronze;

        GameManager.Instance.AddMedal(result);
        SceneManager.LoadScene("Campus_Main");
    }
}
