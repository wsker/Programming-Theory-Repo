using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls movement/behaviour of projectiles
/// </summary>
public class ProjectileMovement : MonoBehaviour
{
    /// <summary>
    /// List of sources of the projectile, to mark who can get hit by it.
    /// </summary>
    public enum Source
    {
        Player,
        Enemy
    }

    // Attributes
    /// <summary>
    /// Direction the projectile moves in
    /// </summary>
    private Vector3 direction = Vector3.forward;
    /// <summary>
    /// Speed of the projectile
    /// </summary>
    private float speed = 10.0f;
    /// <summary>
    /// Lifetime of the projectile in seconds (will be destroyed afterwards)
    /// </summary>
    private float lifetime = 2.0f;
    /// <summary>
    /// Damage inflicted by the projectile.
    /// </summary>
    private int damage = 1;
    /// <summary>
    /// Source of the projectile, which it will never collide with
    /// </summary>
    private Source projectileSource;

    /// <summary>
    /// Used to initialize the projectile after creation.
    /// </summary>
    /// <param name="source">Who created the projectile.</param>
    /// <param name="direction">The direction the projectile should move.</param>
    /// <param name="damage">The damage the projectile does.</param>
    public void Initialize(Source source, Vector3 direction, int damage)
    {
        this.direction = direction;
        this.damage = damage;
        projectileSource = source;
    }

    // Update is called once per frame
    void Update()
    {
        // move projectile
        transform.Translate(direction * speed * Time.deltaTime);
        
        // manage lifetime
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if(go.CompareTag("Wall"))
        {
            // always destroyed when hitting a wall
            Destroy(gameObject);
        }
        else if(projectileSource != Source.Enemy && go.CompareTag("Enemy"))
        {
            // hit an enemy
            go.GetComponent<EnemyMovement>()?.Hit(damage);
            Destroy(gameObject);
        }
        else if (projectileSource != Source.Player && go.CompareTag("Player"))
        {
            // hit the player
            go.GetComponent<PlayerController>()?.Hit(damage);
            Destroy(gameObject);
        }
    }
}
