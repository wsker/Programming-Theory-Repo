using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player, including stats/attributes and state.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Component references
    private Rigidbody rbPlayer;
    public GameObject projectilePrefab;

    // Player attributes
    /// <summary>
    /// Health of the player in absolute units.
    /// </summary>
    [SerializeField] private int health = 6;
    /// <summary>
    /// Movement speed of the player, as Rigidbody velocity.
    /// </summary>
    [SerializeField] private float movementSpeed = 2.0f;
    /// <summary>
    /// Damage of the player character.
    /// </summary>
    [SerializeField] private int damage = 1;
    /// <summary>
    /// Attack speed of the player, as seconds/attack (so lower values mean more attacks/s).
    /// </summary>
    [SerializeField] private float attackSpeed = 0.75f;
    /// <summary>
    /// Internal variable to track the pause between attacks.
    /// </summary>
    private float attackPause = 0;
    
    // Input definitions
    private readonly KeyCode keyMoveUp = KeyCode.W;
    private readonly KeyCode keyMoveLeft = KeyCode.A;
    private readonly KeyCode keyMoveDown = KeyCode.S;
    private readonly KeyCode keyMoveRight = KeyCode.D;

    private readonly KeyCode keyFireUp = KeyCode.UpArrow;
    private readonly KeyCode keyFireLeft = KeyCode.LeftArrow;
    private readonly KeyCode keyFireDown = KeyCode.DownArrow;
    private readonly KeyCode keyFireRight = KeyCode.RightArrow;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackPause > 0)
        {
            // player cannot attack right now
            attackPause -= Time.deltaTime;
        }
        else
        {
            // check for fire button inputs
            float xFire = (Input.GetKeyDown(keyFireRight) ? 1 : 0) - (Input.GetKeyDown(keyFireLeft) ? 1 : 0);
            float zFire = (Input.GetKeyDown(keyFireUp) ? 1 : 0) - (Input.GetKeyDown(keyFireDown) ? 1 : 0);

            if (xFire != 0 || zFire != 0)
            {
                // if player fires: create projectile
                GameObject inst = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
                ProjectileMovement projectile = inst.GetComponent<ProjectileMovement>();
                projectile.Initialize(ProjectileMovement.Source.Player, new Vector3(xFire, 0, zFire), damage);

                attackPause = attackSpeed;  // initiate pause between attacks
            }
        }
    }

    private void FixedUpdate()
    {
        // player movement
        float xMove = (Input.GetKey(keyMoveRight) ? 1 : 0) - (Input.GetKey(keyMoveLeft) ? 1 : 0);
        float zMove = (Input.GetKey(keyMoveUp) ? 1 : 0) - (Input.GetKey(keyMoveDown) ? 1 : 0);
        rbPlayer.velocity = new Vector3(xMove * movementSpeed, 0, zMove * movementSpeed);
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            PlayerDie();
        }
    }

    protected void PlayerDie()
    {
        Destroy(gameObject);
    }
}
