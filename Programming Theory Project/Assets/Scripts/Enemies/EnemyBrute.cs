using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrute : EnemyMovement
{
    /// <summary>
    /// How long the enemy moves (chases the player).
    /// </summary>
    [SerializeField] private float movementDuration = 1.0f;
    /// <summary>
    /// How long the enemy rests between moves.
    /// </summary>
    [SerializeField] private float movementWaitDuration = 0.5f;
    
    /// <summary>
    /// Countdown for current movement phase.
    /// </summary>
    private float movementCountdown;
    /// <summary>
    /// Flag for the current movement state.
    /// </summary>
    private bool isMoving = true;
    
    /// <summary>
    /// Initializies all values of the enemy during Start().
    /// </summary>
    override protected void InitializeEnemy()
    {
        enemyName = "Brute";
        health = 6;
        meleeDamage = 2;
        projectileDamage = 0;
        movementSpeed = 2.0f;
        attackSpeed = 5.0f;
        attackPause = attackSpeed;

        movementCountdown = movementDuration;
    }

    /// <summary>
    /// Chases the player in regular intervalls, resting in between chases.
    /// </summary>
    override protected void SetMovement()
    {
        if(isMoving)
        {
            if(player != null)
            {
                // move straight towards player
                Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
                xMove = direction.x;
                zMove = direction.z;
            }
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
        }
    }

    /// <summary>
    /// This enemy has not direct attack, only melee damage.
    /// </summary>
    override protected void PerformAttack()
    {
        attackPause = attackSpeed;
    }
}
