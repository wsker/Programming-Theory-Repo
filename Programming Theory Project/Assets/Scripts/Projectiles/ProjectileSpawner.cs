using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    /// <summary>
    /// List of available shot types.
    /// </summary>
    public enum ShotType
    {
        Single, // a single shot
        Back,   // two shots, in 180 degrees
        Quad    // four shots, in 90 degrees
    }

    /// <summary>
    /// Prefab for the projectile.
    /// </summary>
    public GameObject projectilePrefab;

    public AudioClip shootSound;
    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Spawns projectiles of the provided type.
    /// </summary>
    /// <param name="shotType">The type of projectile.</param>
    /// <param name="position">Where the projectile is spawned.</param>
    /// <param name="direction">The forward direction of the projectile.</param>
    /// <param name="source">Who shot the projectile.</param>
    /// <param name="damage">How much damage the projectile inflicts.</param>
    public void SpawnProjectile(ShotType shotType, Vector3 position, Vector3 direction, ProjectileMovement.Source source, int damage)   // ABSTRACTION
    {
        // always spawn shot in forward direction
        InstantiateProjectile(projectilePrefab, position, direction, source, damage);

        if(shotType != ShotType.Single)
        {
            // every other type shoots backwards
            
            InstantiateProjectile(projectilePrefab, position, new Vector3(-direction.x, direction.y, -direction.z), source, damage);
        }

        if(shotType == ShotType.Quad)
        {
            // create instances moving in perpendicular directions
            InstantiateProjectile(projectilePrefab, position, new Vector3(direction.z, direction.y, -direction.x), source, damage);
            InstantiateProjectile(projectilePrefab, position, new Vector3(-direction.z, direction.y, direction.x), source, damage);
        }

        audioPlayer.PlayOneShot(shootSound);
    }

    /// <summary>
    /// Instantiates one projectile.
    /// </summary>
    /// <param name="projectile">GameObject to spawn.</param>
    /// <param name="position">Where to spawn it.</param>
    /// <param name="direction">Direction it moves in.</param>
    /// <param name="source">Who shot the projectile.</param>
    /// <param name="damage">Damage of the projectile.</param>
    private void InstantiateProjectile(GameObject projectile, Vector3 position, Vector3 direction, ProjectileMovement.Source source, int damage)
    {
        GameObject inst = Instantiate(projectile, position, projectile.transform.rotation);
        inst.GetComponent<ProjectileMovement>().Initialize(source, direction, damage);
    }
}
