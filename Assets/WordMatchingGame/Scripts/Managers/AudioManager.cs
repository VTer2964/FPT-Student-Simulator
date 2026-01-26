using UnityEngine;

namespace WordMatchingGame.Managers
{
    /// <summary>
    /// Simple audio manager for playing sound effects
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip correctMatchSound;
        [SerializeField] private AudioClip wrongMatchSound;
        [SerializeField] private AudioClip levelCompleteSound;
        [SerializeField] private AudioClip timeUpSound;

        [Header("Settings")]
        [SerializeField] private float sfxVolume = 1f;
        [SerializeField] private float musicVolume = 0.5f;

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

            // Load saved settings
            LoadSettings();
        }

        /// <summary>
        /// Play a sound effect
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip, sfxVolume);
            }
        }

        /// <summary>
        /// Play click sound
        /// </summary>
        public void PlayClick()
        {
            PlaySFX(clickSound);
        }

        /// <summary>
        /// Play correct match sound
        /// </summary>
        public void PlayCorrectMatch()
        {
            PlaySFX(correctMatchSound);
        }

        /// <summary>
        /// Play wrong match sound
        /// </summary>
        public void PlayWrongMatch()
        {
            PlaySFX(wrongMatchSound);
        }

        /// <summary>
        /// Play level complete sound
        /// </summary>
        public void PlayLevelComplete()
        {
            PlaySFX(levelCompleteSound);
        }

        /// <summary>
        /// Play time up sound
        /// </summary>
        public void PlayTimeUp()
        {
            PlaySFX(timeUpSound);
        }

        /// <summary>
        /// Set SFX volume
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            if (sfxSource != null)
                sfxSource.volume = sfxVolume;

            SaveSettings();
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null)
                musicSource.volume = musicVolume;

            SaveSettings();
        }

        /// <summary>
        /// Toggle SFX on/off
        /// </summary>
        public void ToggleSFX(bool enabled)
        {
            if (sfxSource != null)
                sfxSource.mute = !enabled;

            SaveSettings();
        }

        /// <summary>
        /// Toggle music on/off
        /// </summary>
        public void ToggleMusic(bool enabled)
        {
            if (musicSource != null)
                musicSource.mute = !enabled;

            SaveSettings();
        }

        /// <summary>
        /// Save audio settings
        /// </summary>
        private void SaveSettings()
        {
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("SFXEnabled", sfxSource != null && !sfxSource.mute ? 1 : 0);
            PlayerPrefs.SetInt("MusicEnabled", musicSource != null && !musicSource.mute ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load audio settings
        /// </summary>
        private void LoadSettings()
        {
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
                sfxSource.mute = PlayerPrefs.GetInt("SFXEnabled", 1) == 0;
            }

            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
                musicSource.mute = PlayerPrefs.GetInt("MusicEnabled", 1) == 0;
            }
        }
    }
}
