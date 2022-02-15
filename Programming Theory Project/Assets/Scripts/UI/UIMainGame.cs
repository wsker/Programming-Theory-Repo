using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIMainGame : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject waveIntro;

    private TextMeshProUGUI waveIntroText;

    private TextMeshProUGUI healthText;
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        // get components
        waveIntroText = waveIntro.GetComponent<TextMeshProUGUI>();
        healthText = gameplay.transform.Find("Health Text").GetComponent<TextMeshProUGUI>();
        timerText = gameplay.transform.Find("Timer Text").GetComponent<TextMeshProUGUI>();
    }

    // Wave Intro
    public void StartWaveIntro(int wave)
    {
        waveIntroText.text = "Wave " + wave;
        waveIntro.SetActive(true);
    }

    public void StopWaveIntro()
    {
        waveIntro.SetActive(false);
    }

    // Gameplay text

    public void UpdateHealth(int health)
    {
        healthText.text = "Health: " + Mathf.Max(0, health);
    }

    public void UpdateTimer(int timer)
    {
        timerText.text = "Time: " + timer;
    }

    public void ShowGameOverScreen(int reachedWave, bool timeExpired)
    {
        TextMeshProUGUI t1 = gameOver.transform.Find("Reached Wave Text").GetComponent<TextMeshProUGUI>();
        t1.text = "Reached wave: " + reachedWave;
        TextMeshProUGUI t2 = gameOver.transform.Find("Killed By Text").GetComponent<TextMeshProUGUI>();
        t2.text = (timeExpired) ? "Time expired" : "Killed by enemy";
        gameOver.SetActive(true);
    }

    // Button handler

    /// <summary>
    /// Retries with the current settings.
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Goes back to the menu.
    /// </summary>
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
