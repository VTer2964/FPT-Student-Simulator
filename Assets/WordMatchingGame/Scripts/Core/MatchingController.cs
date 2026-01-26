using System.Collections.Generic;
using UnityEngine;
using WordMatchingGame.Data;

namespace WordMatchingGame.Core
{
    /// <summary>
    /// Handles the matching logic between English and Vietnamese words
    /// </summary>
    public class MatchingController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float wrongAnswerResetDelay = 0.5f;

        // Current selections
        private WordButton selectedEnglishButton;
        private WordButton selectedVietnameseButton;

        // Reference to current word pairs
        private List<WordPair> currentWordPairs;

        // Events
        public System.Action<WordButton, WordButton> OnCorrectMatch;
        public System.Action<WordButton, WordButton> OnWrongMatch;
        public System.Action OnAllMatched;

        private int matchedPairsCount = 0;
        private int totalPairs = 0;

        /// <summary>
        /// Initialize with the current level's word pairs
        /// </summary>
        public void Initialize(List<WordPair> wordPairs)
        {
            currentWordPairs = wordPairs;
            totalPairs = wordPairs.Count;
            matchedPairsCount = 0;
            ClearSelections();
        }

        /// <summary>
        /// Handle word button click
        /// </summary>
        public void OnWordButtonClicked(WordButton clickedButton)
        {
            // Ignore if already matched
            if (clickedButton.CurrentState == WordButtonState.Matched)
                return;

            if (clickedButton.IsEnglishWord)
            {
                HandleEnglishWordClick(clickedButton);
            }
            else
            {
                HandleVietnameseWordClick(clickedButton);
            }

            // Check if we have both selections
            if (selectedEnglishButton != null && selectedVietnameseButton != null)
            {
                CheckMatch();
            }
        }

        /// <summary>
        /// Handle English word selection
        /// </summary>
        private void HandleEnglishWordClick(WordButton button)
        {
            // Deselect previous English selection
            if (selectedEnglishButton != null && selectedEnglishButton != button)
            {
                selectedEnglishButton.SetState(WordButtonState.Normal);
            }

            // Toggle selection
            if (selectedEnglishButton == button)
            {
                selectedEnglishButton.SetState(WordButtonState.Normal);
                selectedEnglishButton = null;
            }
            else
            {
                selectedEnglishButton = button;
                selectedEnglishButton.SetState(WordButtonState.Selected);
            }
        }

        /// <summary>
        /// Handle Vietnamese word selection
        /// </summary>
        private void HandleVietnameseWordClick(WordButton button)
        {
            // Deselect previous Vietnamese selection
            if (selectedVietnameseButton != null && selectedVietnameseButton != button)
            {
                selectedVietnameseButton.SetState(WordButtonState.Normal);
            }

            // Toggle selection
            if (selectedVietnameseButton == button)
            {
                selectedVietnameseButton.SetState(WordButtonState.Normal);
                selectedVietnameseButton = null;
            }
            else
            {
                selectedVietnameseButton = button;
                selectedVietnameseButton.SetState(WordButtonState.Selected);
            }
        }

        /// <summary>
        /// Check if the selected pair matches
        /// </summary>
        private void CheckMatch()
        {
            // Validate that both IDs match
            bool isMatch = selectedEnglishButton.WordPairID == selectedVietnameseButton.WordPairID;

            if (isMatch)
            {
                HandleCorrectMatch();
            }
            else
            {
                HandleWrongMatch();
            }
        }

        /// <summary>
        /// Handle correct match
        /// </summary>
        private void HandleCorrectMatch()
        {
            // Set matched state
            selectedEnglishButton.SetState(WordButtonState.Matched);
            selectedVietnameseButton.SetState(WordButtonState.Matched);

            // Disable buttons
            selectedEnglishButton.DisableButton();
            selectedVietnameseButton.DisableButton();

            // Trigger event
            OnCorrectMatch?.Invoke(selectedEnglishButton, selectedVietnameseButton);

            // Fade out after a short delay
            StartCoroutine(FadeOutMatchedPair());

            matchedPairsCount++;

            // Check if all pairs are matched
            if (matchedPairsCount >= totalPairs)
            {
                OnAllMatched?.Invoke();
            }

            // Clear selections
            ClearSelections();
        }

        /// <summary>
        /// Handle wrong match
        /// </summary>
        private void HandleWrongMatch()
        {
            // Set wrong state
            selectedEnglishButton.SetState(WordButtonState.Wrong);
            selectedVietnameseButton.SetState(WordButtonState.Wrong);

            // Trigger event
            OnWrongMatch?.Invoke(selectedEnglishButton, selectedVietnameseButton);

            // Reset after delay
            StartCoroutine(ResetWrongSelection());
        }

        /// <summary>
        /// Fade out matched pair
        /// </summary>
        private System.Collections.IEnumerator FadeOutMatchedPair()
        {
            yield return new WaitForSeconds(0.5f);

            selectedEnglishButton.FadeOut();
            selectedVietnameseButton.FadeOut();
        }

        /// <summary>
        /// Reset wrong selections back to normal
        /// </summary>
        private System.Collections.IEnumerator ResetWrongSelection()
        {
            yield return new WaitForSeconds(wrongAnswerResetDelay);

            if (selectedEnglishButton != null)
                selectedEnglishButton.SetState(WordButtonState.Normal);

            if (selectedVietnameseButton != null)
                selectedVietnameseButton.SetState(WordButtonState.Normal);

            ClearSelections();
        }

        /// <summary>
        /// Clear current selections
        /// </summary>
        private void ClearSelections()
        {
            selectedEnglishButton = null;
            selectedVietnameseButton = null;
        }

        /// <summary>
        /// Reset the controller
        /// </summary>
        public void Reset()
        {
            ClearSelections();
            matchedPairsCount = 0;
        }
    }
}
