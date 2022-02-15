using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the EnemySpawner.
    /// </summary>
    private EnemySpawner enemySpawner;
    /// <summary>
    /// Reference to the PowerUp Spawner.
    /// </summary>
    private PowerUpSpawner powerUpSpawner;
    /// <summary>
    /// Container in hierarchy to collect enemies in
    /// </summary>
    private GameObject enemyContainer;
    private GameObject player;

    private UIMainGame uiMainGame;
    private float waveIntroDuration = 3.0f;
    /// <summary>
    /// How the number of power ups spawned scale up per wave.
    /// Every powerUpWaveScale waves an additional power up is spawned.
    /// </summary>
    private int powerUpWaveScale = 3;
    private float timePerWave = 15.0f;
    private float additionalTimePerWave = 2.5f;
    private float timer = 0;

    private bool pauseSpawning = true;
    private bool isGameRunning = false;

    private AudioSource oneShotPlayer;
    private float oneShotPlayerVolume = 0.15f;
    public AudioClip waveDefeatedClip;

    /// <summary>
    /// The current wave.
    /// </summary>
    public int Wave { get; private set; }   // ENCAPSULATION

    // Start is called before the first frame update
    void Start()
    {
        uiMainGame = GameObject.Find("Canvas").GetComponent<UIMainGame>();
        oneShotPlayer = gameObject.AddComponent<AudioSource>();
        oneShotPlayer.loop = false;
        oneShotPlayer.playOnAwake = false;
        oneShotPlayer.volume = oneShotPlayerVolume;

        // get starting wave
        Wave = WaveManager.Instance?.waves.selected ?? 1;

        player = GameObject.Find("Player");
        enemyContainer = GameObject.Find("Enemies");
        enemySpawner = GetComponent<EnemySpawner>();
        powerUpSpawner = GetComponent<PowerUpSpawner>();
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameRunning)
        {
            timer -= Time.deltaTime;
            uiMainGame.UpdateTimer((int)Mathf.Ceil(timer));
            if(timer <= 0 || player == null)
            {
                GameOver(player != null);
            }
        }
        
        if(!pauseSpawning && enemyContainer.transform.childCount == 0)
        {
            Wave++;
            StartWave();
            oneShotPlayer.PlayOneShot(waveDefeatedClip);
        }
    }

    public void StartWave()
    {
        pauseSpawning = true;
        isGameRunning = false;
        timer += timePerWave + ((Wave - 1) * additionalTimePerWave);
        uiMainGame.UpdateTimer((int)Mathf.Ceil(timer));
        StartCoroutine(WaveIntro(Wave));
    }

    IEnumerator WaveIntro(int wave)
    {
        uiMainGame.StartWaveIntro(wave);
        yield return new WaitForSeconds(waveIntroDuration);
        uiMainGame.StopWaveIntro();
        SpawnWave(wave);
        SpawnPowerUps(wave);
        pauseSpawning = false;
        isGameRunning = true;
    }

    protected void SpawnWave(int wave)
    {
        for(int i = 0; i < wave; i++)
        {
            enemySpawner.SpawnRandomEnemy();
        }
        WaveManager.Instance?.ReportReachedWave(wave);
        Debug.Log("Spawned wave " + wave);
    }

    protected void GameOver(bool timeExpired)
    {
        pauseSpawning = true;
        isGameRunning = false;
        Destroy(player);
        uiMainGame.ShowGameOverScreen(Wave, timeExpired);
    }

    /// <summary>
    /// Spawns power ups based on the current wave.
    /// </summary>
    /// <param name="wave"></param>
    protected void SpawnPowerUps(int wave)
    {
        for(int i = 0; i < 1 + (wave / powerUpWaveScale); i++)
        {
            powerUpSpawner.SpawnRandomPowerUp();
        }
    }
}
