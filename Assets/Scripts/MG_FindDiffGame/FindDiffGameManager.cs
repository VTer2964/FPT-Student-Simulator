using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 


// ← THÊM ENUM NÀY VÀO ĐẦU FILE
public enum FindDiffMedalType { None, Bronze, Silver, Gold }

public class FindDiffGameManager : MonoBehaviour
{
    [Header("Levels")]
    public GameObject[] levelPanels;
    private int currentLevel = 0;
    private int levelsCompleted = 0;

    [Header("Game")]
    public int spotsPerLevel = 5;
    public int foundSpots = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public GameObject winPanel, losePanel;
    public float maxTime = 600f;
    private float timeLeft;
    private Coroutine timerCoroutine;
    private bool gameOver = false;

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;  // ← ĐÚNG (không phải CursorLockState)
        Cursor.visible = true;

        // Tự tạo EventSystem nếu thiếu
        if (FindObjectOfType<EventSystem>() == null)  // ← using đã có, dùng ngắn gọn
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        winPanel.SetActive(false);
        losePanel.SetActive(false);
        SetupLevel(currentLevel);
    }

    void SetupLevel(int levelIndex)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        foreach (var panel in levelPanels)
            panel.SetActive(false);
        if (levelIndex < levelPanels.Length)
            levelPanels[levelIndex].SetActive(true);

        currentLevel = levelIndex;
        foundSpots = 0;
        timeLeft = maxTime;
        UpdateUI();
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void OnSpotFound()
    {
        if (gameOver) return;

        foundSpots++;
        UpdateUI();

        if (foundSpots >= spotsPerLevel)
        {
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            levelsCompleted++;  // Win level này

            if (currentLevel < levelPanels.Length - 1)
            {
                SetupLevel(currentLevel + 1);
            }
            else
            {
                EndGame();
            }
        }
    }

    IEnumerator TimerCoroutine()
    {
        while (timeLeft > 0 && !gameOver)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        if (!gameOver)
        {
            EndGame();  // Timeout
        }
    }

    void UpdateTimerDisplay()
    {
        float displayTime = Mathf.Max(0, timeLeft);
        int minutes = Mathf.FloorToInt(displayTime / 60);
        int seconds = Mathf.FloorToInt(displayTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateUI()
    {
        scoreText.text = $"{foundSpots}/{spotsPerLevel}";
        levelText.text = $"Level: {currentLevel + 1}/{levelPanels.Length}";
        UpdateTimerDisplay();
    }

    void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0;

        // ← TÍNH HUY CHƯƠNG
        FindDiffMedalType medal = FindDiffMedalType.None;
        if (levelsCompleted == 3) medal = FindDiffMedalType.Gold;
        else if (levelsCompleted == 2) medal = FindDiffMedalType.Silver;
        else if (levelsCompleted == 1) medal = FindDiffMedalType.Bronze;

        Debug.Log($"🏅 Medal: {medal} (Completed {levelsCompleted}/3 levels)");

        // ← SAVE MEDAL (chọn 1 cách)
        SaveMedal(medal);  // Cách 1: PlayerPrefs đơn giản


        if (levelsCompleted == 3)
            winPanel.SetActive(true);
        else
            losePanel.SetActive(true);

        StartCoroutine(EndGameWithDelay());
    }

    IEnumerator EndGameWithDelay()
    {
        yield return new WaitForSecondsRealtime(2f);  // Chờ 2s

        // LOAD SINGLE → VỀ 3D CHÍNH
        SceneManager.LoadScene("Campus_Main", LoadSceneMode.Single);

    }
    // ← SAVE MEDAL ĐƠN GIẢN (PlayerPrefs)
    void SaveMedal(FindDiffMedalType medal)
    {
        PlayerPrefs.SetString("FindDiffMedal", medal.ToString());

        if (GameManager.Instance != null)
        {
            switch (medal)
            {
                case FindDiffMedalType.Gold:
                    GameManager.Instance.gold++;
                    break;
                case FindDiffMedalType.Silver:
                    GameManager.Instance.silver++;
                    break;
                case FindDiffMedalType.Bronze:
                    GameManager.Instance.bronze++;
                    break;
            }
        }

        PlayerPrefs.Save();
    }

    public void ExitMinigame()  // ← THÊM HÀM NÀY
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        gameOver = true;

        // RESUME 3D + FIX CURSOR
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;  // ← LOCK LẠI CHO 3D
        Cursor.visible = false;

        // UNLOAD minigame
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }

}
