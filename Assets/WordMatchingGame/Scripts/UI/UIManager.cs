using UnityEngine;
using TMPro;
using WordMatchingGame.Data;

namespace WordMatchingGame.UI
{
    /// <summary>
    /// Manages all UI elements in the game
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private GameObject pausePanel;

        [Header("Game UI Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI accuracyText;

        [Header("Result Panel Elements")]
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI medalText;
        [SerializeField] private GameObject goldMedalIcon;
        [SerializeField] private GameObject silverMedalIcon;
        [SerializeField] private GameObject bronzeMedalIcon;

        /// <summary>
        /// Show a specific panel and hide others
        /// </summary>
        public void ShowPanel(string panelName)
        {
            HideAllPanels();

            switch (panelName.ToLower())
            {
                case "menu":
                    if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
                    break;
                case "game":
                    if (gamePanel != null) gamePanel.SetActive(true);
                    break;
                case "result":
                    if (resultPanel != null) resultPanel.SetActive(true);
                    break;
                case "pause":
                    if (pausePanel != null) pausePanel.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Hide all panels
        /// </summary>
        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(false);
            if (resultPanel != null) resultPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        /// <summary>
        /// Update score display
        /// </summary>
        public void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {score}";
        }

        /// <summary>
        /// Update timer display
        /// </summary>
        public void UpdateTimer(float timeRemaining)
        {
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60f);
                int seconds = Mathf.FloorToInt(timeRemaining % 60f);
                timerText.text = $"Time: {minutes:00}:{seconds:00}";

                // Change color if time is running out
                if (timeRemaining < 10f)
                    timerText.color = Color.red;
                else
                    timerText.color = Color.white;
            }
        }

        /// <summary>
        /// Update level display
        /// </summary>
        public void UpdateLevel(int levelNumber)
        {
            if (levelText != null)
                levelText.text = $"Level {levelNumber + 1}";
        }

        /// <summary>
        /// Update accuracy display
        /// </summary>
        public void UpdateAccuracy(float accuracy)
        {
            if (accuracyText != null)
                accuracyText.text = $"Accuracy: {accuracy:F1}%";
        }

        /// <summary>
        /// Show result panel with medal and score
        /// </summary>
        public void ShowResult(Data.MedalType medal, int medalPoints, int finalScore, float accuracy)
        {
            ShowPanel("result");

            // Update result text
            if (resultTitleText != null)
            {
                if (medal == Data.MedalType.None)
                    resultTitleText.text = "Time's Up!";
                else
                    resultTitleText.text = "Level Complete!";
            }

            if (finalScoreText != null)
                finalScoreText.text = $"Score: {finalScore}\nAccuracy: {accuracy:F1}%";

            // Show medal
            HideAllMedals();
            switch (medal)
            {
                case Data.MedalType.Gold:
                    if (goldMedalIcon != null) goldMedalIcon.SetActive(true);
                    if (medalText != null) medalText.text = $"Gold Medal!\n+{medalPoints} Points";
                    break;
                case Data.MedalType.Silver:
                    if (silverMedalIcon != null) silverMedalIcon.SetActive(true);
                    if (medalText != null) medalText.text = $"Silver Medal!\n+{medalPoints} Points";
                    break;
                case Data.MedalType.Bronze:
                    if (bronzeMedalIcon != null) bronzeMedalIcon.SetActive(true);
                    if (medalText != null) medalText.text = $"Bronze Medal!\n+{medalPoints} Points";
                    break;
                case Data.MedalType.None:
                    if (medalText != null) medalText.text = "Try Again!";
                    break;
            }
        }

        /// <summary>
        /// Hide all medal icons
        /// </summary>
        private void HideAllMedals()
        {
            if (goldMedalIcon != null) goldMedalIcon.SetActive(false);
            if (silverMedalIcon != null) silverMedalIcon.SetActive(false);
            if (bronzeMedalIcon != null) bronzeMedalIcon.SetActive(false);
        }

        /// <summary>
        /// Show pause panel
        /// </summary>
        public void ShowPausePanel()
        {
            if (pausePanel != null)
                pausePanel.SetActive(true);
        }

        /// <summary>
        /// Hide pause panel
        /// </summary>
        public void HidePausePanel()
        {
            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }
}
