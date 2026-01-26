using UnityEngine;

namespace WordMatchingGame.Core
{
    /// <summary>
    /// Enum representing different game states
    /// </summary>
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        LevelComplete,
        GameOver
    }

    /// <summary>
    /// Singleton GameManager that controls the overall game flow
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.Menu;

        [Header("Current Level")]
        [SerializeField] private int currentLevelIndex = 0;

        // Events
        public System.Action<GameState> OnGameStateChanged;
        public System.Action<int> OnLevelChanged;

        public GameState CurrentState => currentState;
        public int CurrentLevelIndex => currentLevelIndex;

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // Load saved progress
            LoadProgress();
        }

        /// <summary>
        /// Change the current game state
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentState == newState) return;

            currentState = newState;
            OnGameStateChanged?.Invoke(currentState);

            Debug.Log($"Game State Changed: {currentState}");
        }

        /// <summary>
        /// Start a new level
        /// </summary>
        public void StartLevel(int levelIndex)
        {
            currentLevelIndex = levelIndex;
            OnLevelChanged?.Invoke(currentLevelIndex);
            SetGameState(GameState.Playing);

            Debug.Log($"Starting Level {currentLevelIndex}");
        }

        /// <summary>
        /// Complete current level and move to next
        /// </summary>
        public void CompleteLevel()
        {
            SetGameState(GameState.LevelComplete);
            SaveProgress();
        }

        /// <summary>
        /// Move to next level
        /// </summary>
        public void NextLevel()
        {
            currentLevelIndex++;
            StartLevel(currentLevelIndex);
        }

        /// <summary>
        /// Retry current level
        /// </summary>
        public void RetryLevel()
        {
            StartLevel(currentLevelIndex);
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMenu()
        {
            SetGameState(GameState.Menu);
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
                Time.timeScale = 0f;
            }
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Save game progress using PlayerPrefs
        /// </summary>
        private void SaveProgress()
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
            PlayerPrefs.Save();
            Debug.Log($"Progress saved: Level {currentLevelIndex}");
        }

        /// <summary>
        /// Load game progress from PlayerPrefs
        /// </summary>
        private void LoadProgress()
        {
            currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
            Debug.Log($"Progress loaded: Level {currentLevelIndex}");
        }

        /// <summary>
        /// Reset all progress
        /// </summary>
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            currentLevelIndex = 0;
            Debug.Log("Progress reset!");
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }
    }
}
