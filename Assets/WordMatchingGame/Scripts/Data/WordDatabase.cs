using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordMatchingGame.Data
{
    /// <summary>
    /// ScriptableObject that stores all word pairs for the game
    /// </summary>
    [CreateAssetMenu(fileName = "WordDatabase", menuName = "Word Matching Game/Word Database")]
    public class WordDatabase : ScriptableObject
    {
        [SerializeField] private List<WordPair> allWords = new List<WordPair>();

        public List<WordPair> AllWords => allWords;

        /// <summary>
        /// Get random word pairs for a level
        /// </summary>
        public List<WordPair> GetRandomWords(int count, int maxDifficulty = 3)
        {
            // Filter by difficulty
            var availableWords = allWords.Where(w => w.Difficulty <= maxDifficulty).ToList();

            if (availableWords.Count < count)
            {
                Debug.LogWarning($"Not enough words available! Requested: {count}, Available: {availableWords.Count}");
                return availableWords;
            }

            // Shuffle and take
            var shuffled = availableWords.OrderBy(x => Random.value).Take(count).ToList();
            return shuffled;
        }

        /// <summary>
        /// Get words by category
        /// </summary>
        public List<WordPair> GetWordsByCategory(string category, int count)
        {
            var categoryWords = allWords.Where(w => w.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase)).ToList();
            return categoryWords.OrderBy(x => Random.value).Take(count).ToList();
        }

        /// <summary>
        /// Get words by difficulty level
        /// </summary>
        public List<WordPair> GetWordsByDifficulty(int difficulty, int count)
        {
            var difficultyWords = allWords.Where(w => w.Difficulty == difficulty).ToList();
            return difficultyWords.OrderBy(x => Random.value).Take(count).ToList();
        }

        /// <summary>
        /// Add a new word pair (for editor use)
        /// </summary>
        public void AddWord(WordPair word)
        {
            if (!allWords.Contains(word))
            {
                allWords.Add(word);
            }
        }

        /// <summary>
        /// Get total word count
        /// </summary>
        public int GetWordCount()
        {
            return allWords.Count;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Helper method to populate sample data for testing
        /// </summary>
        [ContextMenu("Populate Sample Data")]
        private void PopulateSampleData()
        {
            allWords.Clear();

            // Animals
            allWords.Add(new WordPair(1, "Cat", "Mèo", 1, "Animals"));
            allWords.Add(new WordPair(2, "Dog", "Chó", 1, "Animals"));
            allWords.Add(new WordPair(3, "Bird", "Chim", 1, "Animals"));
            allWords.Add(new WordPair(4, "Fish", "Cá", 1, "Animals"));
            allWords.Add(new WordPair(5, "Elephant", "Voi", 2, "Animals"));
            allWords.Add(new WordPair(6, "Tiger", "Hổ", 2, "Animals"));
            allWords.Add(new WordPair(7, "Butterfly", "Bướm", 2, "Animals"));
            allWords.Add(new WordPair(8, "Crocodile", "Cá sấu", 3, "Animals"));

            // Colors
            allWords.Add(new WordPair(9, "Red", "Đỏ", 1, "Colors"));
            allWords.Add(new WordPair(10, "Blue", "Xanh dương", 1, "Colors"));
            allWords.Add(new WordPair(11, "Green", "Xanh lá", 1, "Colors"));
            allWords.Add(new WordPair(12, "Yellow", "Vàng", 1, "Colors"));
            allWords.Add(new WordPair(13, "Purple", "Tím", 2, "Colors"));
            allWords.Add(new WordPair(14, "Orange", "Cam", 2, "Colors"));

            // Food
            allWords.Add(new WordPair(15, "Rice", "Cơm", 1, "Food"));
            allWords.Add(new WordPair(16, "Bread", "Bánh mì", 1, "Food"));
            allWords.Add(new WordPair(17, "Water", "Nước", 1, "Food"));
            allWords.Add(new WordPair(18, "Apple", "Táo", 1, "Food"));
            allWords.Add(new WordPair(19, "Banana", "Chuối", 1, "Food"));
            allWords.Add(new WordPair(20, "Chicken", "Gà", 2, "Food"));

            Debug.Log($"Populated {allWords.Count} sample words!");
        }
#endif
    }
}
