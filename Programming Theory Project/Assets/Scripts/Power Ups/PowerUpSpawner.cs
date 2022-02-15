using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private List<PowerUp> powerUps = new List<PowerUp>();

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

    /// <summary>
    /// Spawns one random power up in a random position.
    /// </summary>
    public void SpawnRandomPowerUp()
    {
        if(powerUps.Count > 0)
        {
            int index = Random.Range(0, powerUps.Count);
            Vector3 pos = GetRandomPosition();
            Instantiate(powerUps[index], pos, powerUps[index].transform.rotation);
        }
    }

    /// <summary>
    /// Returns a random position inside the arena.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-xRange, xRange);
        float z = Random.Range(-zRange, zRange);
        return new Vector3(x, yFixed, z);
    }
}
