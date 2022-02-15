using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : EnemyMovement    // INHERITANCE
{
    /// <summary>
    /// The minimum distance between enemy and player (causes flee state).
    /// </summary>
    [SerializeField] private float minimumDistance = 2.0f;
    /// <summary>
    /// The speed during fleeing.
    /// </summary>
    [SerializeField] private float fleeSpeed = 6.0f;
    /// <summary>
    /// How long the enemy flees before he checks the distance again.
    /// </summary>
    [SerializeField] private float fleeDuration = 0.5f;
    /// <summary>
    /// To store the baseSpeed so movementSpeed can be overwritten when fleeing.
    /// </summary>
    float baseSpeed;
    /// <summary>
    /// Flag if enemy is currently fleeing.
    /// </summary>
    bool fleeing = false;
    /// <summary>
    /// Flag if enemy has found a shooting position.
    /// </summary>
    bool found = false;
    /// <summary>
    /// Countdown during fleeing.
    /// </summary>
    float stateCountdown;
    

    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        baseSpeed = movementSpeed;
    }

    override protected void SetMovement()
    {
        xMove = zMove = 0;
        if (player == null)
        {
            // do nothing when there is no player
            return;
        }
        else if(fleeing == true)
        {
            // flee away from the player
            Vector3 distance = Vector3.Normalize(transform.position - player.transform.position);
            xMove = distance.x;
            zMove = distance.z;
            found = true;   // always shoot when fleeing

            // check if enemy should stop fleeing
            stateCountdown -= Time.deltaTime;
            if(stateCountdown < 0)
            {
                fleeing = false;
                movementSpeed = baseSpeed;
            }
        }
        else
        {
            // check if shooting position is found (also determines if enemy should move towards player on an axis)
            Vector3 distance = player.transform.position - transform.position;
            found = (Mathf.Abs(distance.x) < 0.2 || Mathf.Abs(distance.z) < 0.2);
            // check if enemy should flee
            if (distance.magnitude < minimumDistance)
            {
                movementSpeed = fleeSpeed;
                stateCountdown = fleeDuration;
                fleeing = true;
            }
            else if (!found)
            {
                // seek horizontal or vertical position with player
                if (Mathf.Abs(distance.x) < Mathf.Abs(distance.z))
                {
                    xMove = (distance.x > 0) ? 1 : -1;
                    zMove = 0;
                }
                else
                {
                    zMove = (distance.z > 0) ? 1 : -1;
                    xMove = 0;
                }
            }
        }
    }

    override protected void PerformAttack()
    {
        if (player != null && found)
        {
            // only attack if there is a player and a shootin position was found
            // determine shooting direction
            Vector3 attackDirection = Vector3.Normalize(player.transform.position - transform.position);
            bool attack = false;
            if(Mathf.Abs(attackDirection.x) > 0.9)
            {
                attackDirection.x = (attackDirection.x > 0) ? 1 : -1;
                attackDirection.z = 0;
                attack = true;
            }
            else if (Mathf.Abs(attackDirection.z) > 0.9)
            {
                attackDirection.z = (attackDirection.z > 0) ? 1 : -1;
                attackDirection.x = 0;
                attack = true;
            }
            // if a valid shooting direction was found
            if(attack)
            {
                projectileSpawner.SpawnProjectile(shotType, transform.position, attackDirection, ProjectileMovement.Source.Enemy, projectileDamage);
                attackPause = attackSpeed;
            }
        }
    }
}
