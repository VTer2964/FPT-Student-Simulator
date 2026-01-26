using System.Collections.Generic;
using UnityEngine;

namespace WordMatchingGame.Data
{
    /// <summary>
    /// Configuration data for each level
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData", menuName = "Word Matching Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        [SerializeField] private int levelNumber;
        [SerializeField] private string levelName;

        [Header("Gameplay Settings")]
        [SerializeField] private int wordPairCount = 5; // 5-8 pairs
        [SerializeField] private float timeLimit = 60f; // seconds
        [SerializeField] private int maxDifficulty = 1; // Max difficulty level for words

        [Header("Scoring")]
        [SerializeField] private int pointsPerCorrect = 10;
        [SerializeField] private int pointsPerWrong = -5;
        [SerializeField] private float timePenalty = 2f; // seconds deducted for wrong answer

        [Header("Medal Requirements")]
        [SerializeField] private int goldThreshold = 90; // 90% accuracy or higher
        [SerializeField] private int silverThreshold = 70; // 70-89% accuracy
        // Bronze = complete the level (below 70%)

        [Header("Optional: Specific Words")]
        [SerializeField] private bool useSpecificWords = false;
        [SerializeField] private List<WordPair> specificWords = new List<WordPair>();

        // Properties
        public int LevelNumber => levelNumber;
        public string LevelName => levelName;
        public int WordPairCount => wordPairCount;
        public float TimeLimit => timeLimit;
        public int MaxDifficulty => maxDifficulty;
        public int PointsPerCorrect => pointsPerCorrect;
        public int PointsPerWrong => pointsPerWrong;
        public float TimePenalty => timePenalty;
        public int GoldThreshold => goldThreshold;
        public int SilverThreshold => silverThreshold;
        public bool UseSpecificWords => useSpecificWords;
        public List<WordPair> SpecificWords => specificWords;

        /// <summary>
        /// Calculate medal based on accuracy percentage
        /// </summary>
        public MedalType CalculateMedal(float accuracyPercent)
        {
            if (accuracyPercent >= goldThreshold)
                return MedalType.Gold;
            else if (accuracyPercent >= silverThreshold)
                return MedalType.Silver;
            else
                return MedalType.Bronze;
        }

        /// <summary>
        /// Get points for medal type
        /// </summary>
        public int GetMedalPoints(MedalType medal)
        {
            switch (medal)
            {
                case MedalType.Gold:
                    return 3;
                case MedalType.Silver:
                    return 2;
                case MedalType.Bronze:
                    return 1;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Medal types for level completion
    /// </summary>
    public enum MedalType
    {
        None = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3
    }
}
