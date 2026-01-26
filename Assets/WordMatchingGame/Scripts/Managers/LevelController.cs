using System.Collections.Generic;
using UnityEngine;
using WordMatchingGame.Data;
using WordMatchingGame.Core;

namespace WordMatchingGame.Managers
{
    /// <summary>
    /// Controls the current level gameplay
    /// Manages word spawning, timer, and level completion
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WordDatabase wordDatabase;
        [SerializeField] private LevelData currentLevelData;
        [SerializeField] private MatchingController matchingController;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Spawn Points")]
        [SerializeField] private Transform englishWordsParent;
        [SerializeField] private Transform vietnameseWordsParent;

        [Header("Prefabs")]
        [SerializeField] private GameObject wordButtonPrefab;

        [Header("Timer")]
        [SerializeField] private float currentTime;
        [SerializeField] private bool isTimerRunning = false;

        // Current level words
        private List<WordPair> currentWords;
        private List<WordButton> englishButtons = new List<WordButton>();
        private List<WordButton> vietnameseButtons = new List<WordButton>();

        // Events
        public System.Action<float> OnTimeChanged;
        public System.Action OnTimeUp;
        public System.Action<Data.MedalType, int> OnLevelCompleted;

        public float CurrentTime => currentTime;
        public float TimeRemaining => Mathf.Max(0, currentTime);

        private void Update()
        {
            if (isTimerRunning)
            {
                UpdateTimer();
            }
        }

        /// <summary>
        /// Start a new level
        /// </summary>
        public void StartLevel(LevelData levelData)
        {
            currentLevelData = levelData;

            // Reset
            ClearLevel();
            scoreManager.ResetScore();

            // Get words for this level
            if (levelData.UseSpecificWords && levelData.SpecificWords.Count > 0)
            {
                currentWords = levelData.SpecificWords;
            }
            else
            {
                currentWords = wordDatabase.GetRandomWords(levelData.WordPairCount, levelData.MaxDifficulty);
            }

            // Spawn word buttons
            SpawnWordButtons();

            // Initialize matching controller
            matchingController.Initialize(currentWords);

            // Subscribe to events
            matchingController.OnCorrectMatch += OnCorrectMatch;
            matchingController.OnWrongMatch += OnWrongMatch;
            matchingController.OnAllMatched += OnAllMatched;

            // Start timer
            currentTime = levelData.TimeLimit;
            isTimerRunning = true;

            Debug.Log($"Level Started: {levelData.LevelName}, Words: {currentWords.Count}");
        }

        /// <summary>
        /// Spawn word buttons for English and Vietnamese
        /// </summary>
        private void SpawnWordButtons()
        {
            // Shuffle Vietnamese words for random positioning
            List<WordPair> shuffledVietnamese = new List<WordPair>(currentWords);
            ShuffleList(shuffledVietnamese);

            // Spawn English words (left column)
            for (int i = 0; i < currentWords.Count; i++)
            {
                GameObject buttonObj = Instantiate(wordButtonPrefab, englishWordsParent);
                WordButton wordButton = buttonObj.GetComponent<WordButton>();

                wordButton.Initialize(currentWords[i].EnglishWord, currentWords[i].ID, true);
                wordButton.OnWordClicked += matchingController.OnWordButtonClicked;

                englishButtons.Add(wordButton);
            }

            // Spawn Vietnamese words (right column, shuffled)
            for (int i = 0; i < shuffledVietnamese.Count; i++)
            {
                GameObject buttonObj = Instantiate(wordButtonPrefab, vietnameseWordsParent);
                WordButton wordButton = buttonObj.GetComponent<WordButton>();

                wordButton.Initialize(shuffledVietnamese[i].VietnameseWord, shuffledVietnamese[i].ID, false);
                wordButton.OnWordClicked += matchingController.OnWordButtonClicked;

                vietnameseButtons.Add(wordButton);
            }
        }

        /// <summary>
        /// Shuffle a list (Fisher-Yates algorithm)
        /// </summary>
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Update the timer
        /// </summary>
        private void UpdateTimer()
        {
            currentTime -= Time.deltaTime;
            OnTimeChanged?.Invoke(currentTime);

            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                OnTimeUp?.Invoke();
                EndLevel(false);
            }
        }

        /// <summary>
        /// Handle correct match
        /// </summary>
        private void OnCorrectMatch(WordButton english, WordButton vietnamese)
        {
            scoreManager.AddCorrectMatch(currentLevelData.PointsPerCorrect);
        }

        /// <summary>
        /// Handle wrong match
        /// </summary>
        private void OnWrongMatch(WordButton english, WordButton vietnamese)
        {
            scoreManager.AddWrongMatch(currentLevelData.PointsPerWrong);

            // Apply time penalty
            currentTime = Mathf.Max(0, currentTime - currentLevelData.TimePenalty);
        }

        /// <summary>
        /// Handle all words matched
        /// </summary>
        private void OnAllMatched()
        {
            isTimerRunning = false;
            EndLevel(true);
        }

        /// <summary>
        /// End the level and calculate results
        /// </summary>
        private void EndLevel(bool completed)
        {
            isTimerRunning = false;

            if (completed)
            {
                // Calculate medal
                Data.MedalType medal = scoreManager.CalculateMedal(currentLevelData);
                int medalPoints = scoreManager.GetMedalPoints(medal, currentLevelData);

                // Add to total score
                scoreManager.AddToTotalScore(medalPoints);

                // Trigger completion event
                OnLevelCompleted?.Invoke(medal, medalPoints);

                Debug.Log($"Level Completed! Medal: {medal}, Points: {medalPoints}");
            }
            else
            {
                Debug.Log("Time's up! Level failed.");
                OnLevelCompleted?.Invoke(Data.MedalType.None, 0);
            }

            // Unsubscribe from events
            matchingController.OnCorrectMatch -= OnCorrectMatch;
            matchingController.OnWrongMatch -= OnWrongMatch;
            matchingController.OnAllMatched -= OnAllMatched;
        }

        /// <summary>
        /// Clear all word buttons
        /// </summary>
        private void ClearLevel()
        {
            foreach (var button in englishButtons)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }

            foreach (var button in vietnameseButtons)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }

            englishButtons.Clear();
            vietnameseButtons.Clear();
        }

        /// <summary>
        /// Pause the timer
        /// </summary>
        public void PauseTimer()
        {
            isTimerRunning = false;
        }

        /// <summary>
        /// Resume the timer
        /// </summary>
        public void ResumeTimer()
        {
            isTimerRunning = true;
        }

        private void OnDestroy()
        {
            // Cleanup
            if (matchingController != null)
            {
                matchingController.OnCorrectMatch -= OnCorrectMatch;
                matchingController.OnWrongMatch -= OnWrongMatch;
                matchingController.OnAllMatched -= OnAllMatched;
            }
        }
    }
}
