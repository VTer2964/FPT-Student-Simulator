using System;
using UnityEngine;

namespace WordMatchingGame.Data
{
    /// <summary>
    /// Represents a single English-Vietnamese word pair
    /// </summary>
    [Serializable]
    public class WordPair
    {
        [SerializeField] private string englishWord;
        [SerializeField] private string vietnameseWord;
        [SerializeField] private int id;
        [SerializeField] private int difficulty; // 1 = Easy, 2 = Medium, 3 = Hard
        [SerializeField] private string category; // Optional: "Animals", "Food", "Colors", etc.

        public string EnglishWord => englishWord;
        public string VietnameseWord => vietnameseWord;
        public int ID => id;
        public int Difficulty => difficulty;
        public string Category => category;

        public WordPair(int id, string englishWord, string vietnameseWord, int difficulty = 1, string category = "General")
        {
            this.id = id;
            this.englishWord = englishWord;
            this.vietnameseWord = vietnameseWord;
            this.difficulty = difficulty;
            this.category = category;
        }

        /// <summary>
        /// Check if the provided Vietnamese word matches this pair
        /// </summary>
        public bool IsMatch(string vietnameseWordToCheck)
        {
            return vietnameseWord.Equals(vietnameseWordToCheck, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"[{id}] {englishWord} = {vietnameseWord} (Difficulty: {difficulty}, Category: {category})";
        }
    }
}
