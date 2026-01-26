using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WordMatchingGame.Core
{
    /// <summary>
    /// Represents the state of a word button
    /// </summary>
    public enum WordButtonState
    {
        Normal,
        Selected,
        Matched,
        Wrong
    }

    /// <summary>
    /// Component attached to each word button
    /// Handles click events, visual feedback, and animations
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class WordButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI wordText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Animator animator;

        [Header("Visual Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color correctColor = Color.green;
        [SerializeField] private Color wrongColor = Color.red;

        // State
        private WordButtonState currentState = WordButtonState.Normal;
        private string wordValue;
        private int wordPairID;
        private bool isEnglishWord;

        // Events
        public System.Action<WordButton> OnWordClicked;

        public WordButtonState CurrentState => currentState;
        public string WordValue => wordValue;
        public int WordPairID => wordPairID;
        public bool IsEnglishWord => isEnglishWord;

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();

            if (animator == null)
                animator = GetComponent<Animator>();

            button.onClick.AddListener(OnButtonClick);
        }

        /// <summary>
        /// Initialize the word button with data
        /// </summary>
        public void Initialize(string word, int pairID, bool isEnglish)
        {
            wordValue = word;
            wordPairID = pairID;
            isEnglishWord = isEnglish;

            if (wordText != null)
                wordText.text = word;

            SetState(WordButtonState.Normal);
        }

        /// <summary>
        /// Handle button click
        /// </summary>
        private void OnButtonClick()
        {
            if (currentState == WordButtonState.Matched)
                return; // Already matched, ignore clicks

            OnWordClicked?.Invoke(this);

            // Play click animation
            if (animator != null)
                animator.SetTrigger("Click");
        }

        /// <summary>
        /// Set the visual state of the button
        /// </summary>
        public void SetState(WordButtonState newState)
        {
            currentState = newState;

            if (backgroundImage == null) return;

            switch (currentState)
            {
                case WordButtonState.Normal:
                    backgroundImage.color = normalColor;
                    if (animator != null)
                        animator.SetTrigger("Normal");
                    break;

                case WordButtonState.Selected:
                    backgroundImage.color = selectedColor;
                    if (animator != null)
                        animator.SetTrigger("Select");
                    break;

                case WordButtonState.Matched:
                    backgroundImage.color = correctColor;
                    if (animator != null)
                        animator.SetTrigger("Correct");
                    break;

                case WordButtonState.Wrong:
                    backgroundImage.color = wrongColor;
                    if (animator != null)
                        animator.SetTrigger("Wrong");
                    break;
            }
        }

        /// <summary>
        /// Disable the button (for matched pairs)
        /// </summary>
        public void DisableButton()
        {
            button.interactable = false;
        }

        /// <summary>
        /// Enable the button
        /// </summary>
        public void EnableButton()
        {
            button.interactable = true;
        }

        /// <summary>
        /// Fade out animation (when matched)
        /// </summary>
        public void FadeOut()
        {
            if (animator != null)
                animator.SetTrigger("FadeOut");
            else
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (button == null)
                button = GetComponent<Button>();

            if (wordText == null)
                wordText = GetComponentInChildren<TextMeshProUGUI>();

            if (backgroundImage == null)
                backgroundImage = GetComponent<Image>();

            if (animator == null)
                animator = GetComponent<Animator>();
        }
#endif
    }
}
