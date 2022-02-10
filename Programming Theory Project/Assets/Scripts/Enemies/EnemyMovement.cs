using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // component references
    private Rigidbody rbEnemy;
    private GameObject player;
    public GameObject projectilePrefab;

    // Attributes
    [SerializeField] private string enemyName;
    [SerializeField] private int health = 3;
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float attackSpeed;

    // internal variables
    private float attackPause = 0;
    private float xMove = 0;
    private float zMove = 0;

    // Default behaviour code for all enemies

    void Start()
    {
        InitializeEnemy();
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
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
        // enemy movement
        SetMovement();
        rbEnemy.velocity = new Vector3(xMove * movementSpeed, 0, zMove * movementSpeed);
    }

    public void Hit(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            EnemyDie();
        }
    }

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
        enemyName = "Enemy";
        health = 3;
        meleeDamage = 1;
        projectileDamage = 1;
        movementSpeed = 2.0f;
        attackSpeed = 1.0f;
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
            GameObject inst = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            ProjectileMovement projectile = inst.GetComponent<ProjectileMovement>();
            projectile.Initialize(ProjectileMovement.Source.Enemy, attackDirection, projectileDamage);
        }

        attackPause = attackSpeed;
    }
}
