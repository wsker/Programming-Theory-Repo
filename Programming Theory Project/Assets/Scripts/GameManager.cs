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
    /// Container in hierarchy to collect enemies in
    /// </summary>
    private GameObject enemyContainer;

    private UIMainGame uiMainGame;
    private float waveIntroDuration = 1.0f;

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
}
