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

    private bool pauseSpawning = false;

    public int Wave { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        enemyContainer = GameObject.Find("Enemies");
        enemySpawner = GetComponent<EnemySpawner>();
        if(!pauseSpawning) SpawnNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyContainer.transform.childCount == 0)
        {
            if (!pauseSpawning) SpawnNextWave();
        }
    }

    protected void SpawnNextWave()
    {
        Wave++;
        for(int i = 0; i < Wave; i++)
        {
            enemySpawner.SpawnRandomEnemy();
        }
    }
}
