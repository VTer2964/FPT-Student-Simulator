using UnityEngine;
using UnityEngine.UI;
using WordMatchingGame.Core;
using WordMatchingGame.Managers;

namespace WordMatchingGame.UI
{
    /// <summary>
    /// Main controller that connects all systems together
    /// Handles button clicks and game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Core.GameManager gameManager;
        [SerializeField] private Managers.LevelController levelController;
        [SerializeField] private Managers.ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;

        [Header("Level Data")]
        [SerializeField] private Data.LevelData[] allLevels;

        private void Start()
        {
            // Subscribe to events
            if (levelController != null)
            {
                levelController.OnTimeChanged += OnTimeChanged;
                levelController.OnLevelCompleted += OnLevelCompleted;
            }

            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged += OnScoreChanged;
                scoreManager.OnAccuracyChanged += OnAccuracyChanged;
            }

            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += OnGameStateChanged;
                gameManager.OnLevelChanged += OnLevelChanged;
            }

            // Show main menu
            uiManager.ShowPanel("menu");
        }

        #region Button Handlers

        /// <summary>
        /// Start button clicked
        /// </summary>
        public void OnStartButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.StartLevel(0);
            }
        }

        /// <summary>
        /// Continue button clicked (resume from saved level)
        /// </summary>
        public void OnContinueButtonClicked()
        {
            if (gameManager != null)
            {
                int savedLevel = gameManager.CurrentLevelIndex;
                gameManager.StartLevel(savedLevel);
            }
        }

        /// <summary>
        /// Next level button clicked
        /// </summary>
        public void OnNextLevelButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.NextLevel();
            }
        }

        /// <summary>
        /// Retry button clicked
        /// </summary>
        public void OnRetryButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.RetryLevel();
            }
        }

        /// <summary>
        /// Menu button clicked
        /// </summary>
        public void OnMenuButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.ReturnToMenu();
            }
        }

        /// <summary>
        /// Pause button clicked
        /// </summary>
        public void OnPauseButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.PauseGame();
                uiManager.ShowPausePanel();
                levelController.PauseTimer();
            }
        }

        /// <summary>
        /// Resume button clicked
        /// </summary>
        public void OnResumeButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.ResumeGame();
                uiManager.HidePausePanel();
                levelController.ResumeTimer();
            }
        }

        /// <summary>
        /// Quit button clicked
        /// </summary>
        public void OnQuitButtonClicked()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle game state changes
        /// </summary>
        private void OnGameStateChanged(Core.GameState newState)
        {
            switch (newState)
            {
                case Core.GameState.Menu:
                    uiManager.ShowPanel("menu");
                    break;

                case Core.GameState.Playing:
                    uiManager.ShowPanel("game");
                    break;

                case Core.GameState.LevelComplete:
                    // Result panel will be shown by OnLevelCompleted
                    break;

                case Core.GameState.Paused:
                    uiManager.ShowPausePanel();
                    break;
            }
        }

        /// <summary>
        /// Handle level changes
        /// </summary>
        private void OnLevelChanged(int levelIndex)
        {
            // Update UI
            uiManager.UpdateLevel(levelIndex);

            // Start the level
            if (levelIndex < allLevels.Length)
            {
                levelController.StartLevel(allLevels[levelIndex]);
            }
            else
            {
                Debug.LogWarning($"Level {levelIndex} not found! Total levels: {allLevels.Length}");
            }
        }

        /// <summary>
        /// Handle score changes
        /// </summary>
        private void OnScoreChanged(int newScore)
        {
            uiManager.UpdateScore(newScore);
        }

        /// <summary>
        /// Handle accuracy changes
        /// </summary>
        private void OnAccuracyChanged(float accuracy)
        {
            uiManager.UpdateAccuracy(accuracy);
        }

        /// <summary>
        /// Handle time changes
        /// </summary>
        private void OnTimeChanged(float timeRemaining)
        {
            uiManager.UpdateTimer(timeRemaining);
        }

        /// <summary>
        /// Handle level completion
        /// </summary>
        private void OnLevelCompleted(Data.MedalType medal, int medalPoints)
        {
            // Show result panel
            uiManager.ShowResult(medal, medalPoints, scoreManager.CurrentScore, scoreManager.Accuracy);

            // Complete level in game manager
            gameManager.CompleteLevel();
        }

        #endregion

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (levelController != null)
            {
                levelController.OnTimeChanged -= OnTimeChanged;
                levelController.OnLevelCompleted -= OnLevelCompleted;
            }

            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged -= OnScoreChanged;
                scoreManager.OnAccuracyChanged -= OnAccuracyChanged;
            }

            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
                gameManager.OnLevelChanged -= OnLevelChanged;
            }
        }
    }
}
