using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRager : EnemyMovement // INHERITANCE
{
    /// <summary>
    /// How long the enemy moves.
    /// </summary>
    [SerializeField] private float movementDuration = 1.0f;
    /// <summary>
    /// How long the enemy waits between moves.
    /// </summary>
    [SerializeField] private float movementWaitDuration = 1.0f;
    /// <summary>
    /// How quickly the enemy moves initially.
    /// </summary>
    [SerializeField] private float speedBoost = 5;

    // internal variables
    /// <summary>
    /// Countdown for current move mode (move/wait).
    /// </summary>
    private float movementCountdown;
    /// <summary>
    /// Flag what the current move mode is.
    /// </summary>
    private bool isMoving = true;
    /// <summary>
    /// The direction the enemy is currently moving in.
    /// </summary>
    private Vector3 direction;
    /// <summary>
    /// If the next shot is in alternate direction.
    /// </summary>
    private bool alternateShot = false;

    /// <summary>
    /// Moves in short bursts.
    /// </summary>
    override protected void SetMovement()
    {
        if (isMoving)
        {
            // move in determined direction, with a speed boost
            float boost = speedBoost / (1/movementCountdown);
            xMove = direction.x * boost;
            zMove = direction.z * boost;
        }
        else
        {
            // stand still
            xMove = 0;
            zMove = 0;
        }

        // check if behaviour switches between moving/standing still
        movementCountdown -= Time.deltaTime;
        if (movementCountdown <= 0)
        {
            isMoving = !isMoving;
            movementCountdown = (isMoving) ? movementDuration : movementWaitDuration;
            if(isMoving)
            {
                direction = GetRandomDirection();
            }
        }
    }

    /// <summary>
    /// Determines a random direction to move in.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        return new Vector3(x, 0, z);
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
        if(collision.gameObject.CompareTag("Wall"))
        {
            // when colliding with wall: bounce off it
            ContactPoint point = collision.GetContact(0);
            Vector3 colDirection = point.normal;
            
            float x = direction.x;
            float z = direction.z;
            if(0.1f < colDirection.x || colDirection.x < -0.01f)
            {
                 x *= -1;
            }
            if(0.1f < colDirection.z || colDirection.z < -0.01f)
            {
                z *= -1;
            }
            direction = new Vector3(x, direction.y, z);
        }
    }

    /// <summary>
    /// Shots 4 projectiles, alternating between straight and diagonally.
    /// </summary>
    override protected void PerformAttack()
    {
        if (player != null)
        {
            Vector3 attackDirection = (alternateShot) ? new Vector3(0.5f, 0, 0.5f) : Vector3.forward;
            alternateShot = !alternateShot;
            projectileSpawner.SpawnProjectile(shotType, transform.position, attackDirection, ProjectileMovement.Source.Enemy, projectileDamage);
        }

        attackPause = attackSpeed;
    }
}
