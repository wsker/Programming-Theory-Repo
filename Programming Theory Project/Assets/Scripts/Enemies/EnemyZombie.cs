using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : EnemyMovement    // INHERITANCE
{
    /// <summary>
    /// Range on the x axis to find a random position for an enemy.
    /// </summary>
    private readonly float xRange = 8.0f;
    /// <summary>
    /// Range on the z axis to find a random position for an enemy.
    /// </summary>
    private readonly float zRange = 4.0f;
    private Vector3 movementTarget;

    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        movementTarget = ChooseRandomPosition();
    }

    override protected void SetMovement()
    {
        xMove = 0;
        zMove = 0;
        Vector3 distance = movementTarget - transform.position;
        if(0.2 < distance.x || distance.x < -0.2)
        {
            xMove = (distance.x > 0) ? 1 : -1;
        }
        else if(0.2 < distance.z || distance.z < -0.2)
        {
            zMove = (distance.z > 0) ? 1 : -1;
        }
        else
        {
            movementTarget = ChooseRandomPosition();
            SetMovement();  // choose movement direction for new position, or even new position should they overlap
        }
    }

    /// <summary>
    /// Chooses a random position within the defined range, with only one 
    /// axis (x or z) changing.
    /// </summary>
    /// <returns></returns>
    private Vector3 ChooseRandomPosition()
    {
        float x = transform.position.x;
        float z = transform.position.z;
        if (Random.Range(0, 2) == 0)
        {
            x = Random.Range(-xRange, xRange);
        }
        else
        {
            z = Random.Range(-zRange, zRange);
        }
        return new Vector3(x, transform.position.y, z);
    }

    /// <summary>
    /// Simply shoots a backshot aimed in the current movement direction.
    /// </summary>
    override protected void PerformAttack()
    {
        Vector3 attackDirection = Vector3.Normalize(movementTarget - transform.position);
        projectileSpawner.SpawnProjectile(shotType, transform.position, attackDirection, ProjectileMovement.Source.Enemy, projectileDamage);

        attackPause = attackSpeed;
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Wall"))
        {
            // when colliding with wall: choose new position
            movementTarget = ChooseRandomPosition();
        }
    }
}
