using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// Container in hierarchy to collect enemies in
    /// </summary>
    private GameObject enemyContainer;
    
    /// <summary>
    /// List of different enemies that can be spawned
    /// </summary>
    public List<GameObject> enemyList;

    /// <summary>
    /// Range on the x axis to find a random position for an enemy.
    /// </summary>
    private readonly float xRange = 8.0f;
    /// <summary>
    /// Range on the z axis to find a random position for an enemy.
    /// </summary>
    private readonly float zRange = 4.0f;
    /// <summary>
    /// Random position for enemies use a fixed position on the y axis.
    /// </summary>
    private readonly float yFixed = 0.5f;

    public void Start()
    {
        enemyContainer = GameObject.Find("Enemies");
    }

    /// <summary>
    /// Spawns the specified enemy with a random position.
    /// </summary>
    /// <param name="index">Index of the enemy in the list of available enemies.</param>
    public void SpawnEnemy(int index)
    {
        // check if the enemy-index (and any enemies) exist
        if(enemyList.Count == 0)
        {
            Debug.Log("SpawnEnemy: enemyList is empty");
            return;
        }
        else if(index >= enemyList.Count)
        {
            Debug.Log("SpawnEnemy index out of range, defaulting to first");
            index = 0;
        }

        // create enemy in random position
        GameObject inst = Instantiate(enemyList[index]);
        if (enemyContainer != null) inst.transform.parent = enemyContainer.transform;
        inst.transform.position = GetRandomPosition();
    }

    public void SpawnRandomEnemy()
    {
        int index = Random.Range(0, enemyList.Count);
        SpawnEnemy(index);
    }

    /// <summary>
    /// Generates a random position within a defined range.
    /// </summary>
    /// <returns>Position as a Vector3.</returns>
    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-xRange, xRange);
        float z = Random.Range(-zRange, zRange);
        return new Vector3(x, yFixed, z);
    }
}
