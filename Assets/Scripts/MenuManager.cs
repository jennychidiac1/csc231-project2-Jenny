using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static bool skipTitle;
    private static float volume = 0.8f;
    private static bool muted;

    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerController player;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle muteToggle;

    private bool titleShowing;
    private bool paused;
    private bool wasMovable;

    void Start()
    {
        pausePanel.SetActive(false);
        volumeSlider.SetValueWithoutNotify(volume);
        muteToggle.SetIsOnWithoutNotify(muted);
        ApplyVolume();

        if (skipTitle)
        {
            skipTitle = false;
            titlePanel.SetActive(false);
        }
        else
        {
            titleShowing = true;
            titlePanel.SetActive(true);
            player.canMove = false;
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (titleShowing) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        if (kb.pKey.wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame)
        {
            if (paused) Resume();
            else if (!GameFlow.Instance.Ended) Pause();
        }
    }

    public void StartGame()
    {
        titleShowing = false;
        titlePanel.SetActive(false);
        player.canMove = true;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        paused = true;
        wasMovable = player.canMove;
        player.canMove = false;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        player.canMove = wasMovable;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        skipTitle = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnVolumeChanged(float value)
    {
        volume = value;
        ApplyVolume();
    }

    public void OnMuteChanged(bool isMuted)
    {
        muted = isMuted;
        ApplyVolume();
    }

    void ApplyVolume()
    {
        AudioListener.volume = muted ? 0f : volume;
    }
}