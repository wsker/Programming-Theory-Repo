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

    private UIMainGame uiMainGame;
    private float waveIntroDuration = 1.0f;
    /// <summary>
    /// How the number of power ups spawned scale up per wave.
    /// Every powerUpWaveScale waves an additional power up is spawned.
    /// </summary>
    private int powerUpWaveScale = 3;

    private bool pauseSpawning = false;

    /// <summary>
    /// The current wave.
    /// </summary>
    public int Wave { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        uiMainGame = GameObject.Find("Canvas").GetComponent<UIMainGame>();
        // get starting wave
        Wave = WaveManager.Instance?.waves.selected ?? 1;
        
        enemyContainer = GameObject.Find("Enemies");
        enemySpawner = GetComponent<EnemySpawner>();
        powerUpSpawner = GetComponent<PowerUpSpawner>();
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pauseSpawning && enemyContainer.transform.childCount == 0)
        {
            Wave++;
            StartWave();
        }
    }

    public void StartWave()
    {
        pauseSpawning = true;
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
