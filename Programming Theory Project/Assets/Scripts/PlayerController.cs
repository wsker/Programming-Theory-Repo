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
    private ProjectileSpawner projectileSpawner;
    private UIMainGame uiMainGame;
    public AudioSource audioSource;
    private Renderer playerRenderer;

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
    [SerializeField] private float attackSpeed = 0.5f;
    /// <summary>
    /// Internal variable to track the pause between attacks.
    /// </summary>
    private float attackPause = 0;
    [SerializeField] private ProjectileSpawner.ShotType shotType = ProjectileSpawner.ShotType.Single;
    /// <summary>
    /// The currently picked up power up.
    /// </summary>
    [SerializeField] private PowerUp powerUp = null;

    public AudioClip powerUpPickUpSound;
    public AudioClip gettingHitSound;
    public GameObject deathParticles;

    /// <summary>
    /// How long the player is invulnerable after a hit.
    /// </summary>
    private float invulnerableAfterHitTime = 1.5f;
    /// <summary>
    /// Flag if the player is currently invulnerable.
    /// </summary>
    private bool isInvulnerable = false;

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
        audioSource = GetComponent<AudioSource>();
        playerRenderer = GetComponent<Renderer>();
        projectileSpawner = GameObject.Find("ProjectileSpawner").GetComponent<ProjectileSpawner>();
        uiMainGame = GameObject.Find("Canvas").GetComponent<UIMainGame>();
        uiMainGame.UpdateHealth(health);
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
                // if player fires:
                // apply power up to projectile
                ProjectileSpawner.ShotType st = (powerUp == null) ? shotType : powerUp.ShotType;
                int d = (damage + (powerUp?.Damage ?? 0));
                float ap = (attackSpeed - (powerUp?.AttackSpeed ?? 0));
                // create projectile
                projectileSpawner.SpawnProjectile(st, transform.position, new Vector3(xFire, 0, zFire), ProjectileMovement.Source.Player, d);
                attackPause = ap;  // initiate pause between attacks
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
        if(!isInvulnerable)
        {
            health -= damage;
            audioSource.PlayOneShot(gettingHitSound);
            uiMainGame.UpdateHealth(health);
            if (health <= 0)
            {
                PlayerDie();
            }
            else
            {
                StartCoroutine(InvulnerabilityState(invulnerableAfterHitTime));
            }
        }
    }

    /// <summary>
    /// Manages the time the player is invulnerable.
    /// </summary>
    /// <param name="time">For how long the invulnerability lasts.</param>
    /// <returns></returns>
    IEnumerator InvulnerabilityState(float time)
    {
        isInvulnerable = true;
        float blinkFrequency = 0.15f;
        for(float i = 0; i < time; i += blinkFrequency)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(Mathf.Min(blinkFrequency, time - i));
        }
        playerRenderer.enabled = true;
        isInvulnerable = false;
    }

    protected void PlayerDie()
    {
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        Destroy(gameObject);
    }

    /// <summary>
    /// Attaches a PowerUp to the player. If the player already has a power up,
    /// it will be replaced.
    /// </summary>
    /// <param name="powerUp">The PowerUp to attach.</param>
    public void AttachPowerUp(PowerUp powerUp)
    {
        this.powerUp = powerUp;
        health += powerUp.Health;
        audioSource.PlayOneShot(powerUpPickUpSound);
    }
}
