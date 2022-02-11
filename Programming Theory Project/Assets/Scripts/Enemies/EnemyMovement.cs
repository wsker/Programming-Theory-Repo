using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // component references
    private Rigidbody rbEnemy;
    protected GameObject player;
    protected ProjectileSpawner projectileSpawner;

    // Attributes
    [SerializeField] protected string enemyName;
    [SerializeField] protected int health = 3;
    [SerializeField] protected int meleeDamage = 1;
    [SerializeField] protected int projectileDamage = 1;
    [SerializeField] protected float movementSpeed = 2.0f;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected ProjectileSpawner.ShotType shotType = ProjectileSpawner.ShotType.Single;

    // internal variables
    /// <summary>
    /// How long until the next attack.
    /// </summary>
    protected float attackPause = 0;
    /// <summary>
    /// Current movement on the x axis.
    /// </summary>
    protected float xMove = 0;
    /// <summary>
    /// Current movement on the z axis.
    /// </summary>
    protected float zMove = 0;
    /// <summary>
    /// How long the enemy bounces away from the player (after a melee hit)
    /// </summary>
    protected float bounceAwayDuration = 0.3f;
    /// <summary>
    /// Running counter for the bounce.
    /// </summary>
    protected float bounceAwayFromPlayer = 0;

    // Default behaviour code for all enemies

    void Start()
    {
        InitializeEnemy();
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        projectileSpawner = GameObject.Find("ProjectileSpawner").GetComponent<ProjectileSpawner>();
    }

    void Update()
    {
        // perform attack if it is not on pause
        if(attackPause > 0)
        {
            attackPause -= Time.deltaTime;
        }
        else
        {
            PerformAttack();
        }
    }

    private void FixedUpdate()
    {
        if (bounceAwayFromPlayer > 0 && player != null)
        {
            // if the enemy is currently bouncing away from the player
            Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
            xMove = -direction.x;
            zMove = -direction.z;
            bounceAwayFromPlayer -= Time.fixedDeltaTime;
        }
        else
        {
            // determine enemy movement
            SetMovement();
        }
        // perform movement
        rbEnemy.velocity = new Vector3(xMove * movementSpeed, 0, zMove * movementSpeed);
    }

    /// <summary>
    /// Hit the enemy with the specified damage.
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            EnemyDie();
        }
    }

    /// <summary>
    /// Perform all actions upond enemy death.
    /// </summary>
    protected void EnemyDie()
    {
        Destroy(gameObject);
    }

    // Methods to override to change behaviour of enemies
    /// <summary>
    /// Initializies all values of the enemy during Start().
    /// </summary>
    virtual protected void InitializeEnemy()
    {
        attackPause = attackSpeed;
    }

    /// <summary>
    /// This is called during FixedUpdate and sets where the enemy should move on the 
    /// x and z axis.
    /// </summary>
    virtual protected void SetMovement()
    {

    }

    /// <summary>
    /// This is called during Update if the attack is not on pause and is intended to perform the
    /// attack by e.g. spawning a projectile.
    /// </summary>
    virtual protected void PerformAttack()
    {
        if(player != null)
        {
            Vector3 attackDirection = Vector3.Normalize(player.transform.position - transform.position);
            projectileSpawner.SpawnProjectile(shotType, transform.position, attackDirection, ProjectileMovement.Source.Enemy, projectileDamage);
        }

        attackPause = attackSpeed;
    }

    virtual protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // perform melee hit on player and bounce away from him
            collision.gameObject.GetComponent<PlayerController>().Hit(meleeDamage);
            bounceAwayFromPlayer = bounceAwayDuration;
        }
    }
}
