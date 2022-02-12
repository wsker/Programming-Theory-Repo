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

    // Start is called before the first frame update
    void Start()
    {
        // get components
        waveIntroText = waveIntro.GetComponent<TextMeshProUGUI>();
        healthText = gameplay.transform.Find("Health Text").GetComponent<TextMeshProUGUI>();
        timerText = gameplay.transform.Find("Timer Text").GetComponent<TextMeshProUGUI>();
        // initialize visibility
        gameOver.SetActive(false);
        gameplay.SetActive(true);
        waveIntro.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        healthText.text = "Health: " + health;
    }

    public void UpdateTimer(int timer)
    {
        timerText.text = "Time: " + timer;
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
