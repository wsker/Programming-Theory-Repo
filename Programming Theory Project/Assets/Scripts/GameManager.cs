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

    private int wave = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyContainer = GameObject.Find("Enemies");
        enemySpawner = GetComponent<EnemySpawner>();
        SpawnNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyContainer.transform.childCount == 0)
        {
            SpawnNextWave();
        }
    }

    protected void SpawnNextWave()
    {
        wave++;
        for(int i = 0; i < wave; i++)
        {
            enemySpawner.SpawnRandomEnemy();
        }
    }
}
