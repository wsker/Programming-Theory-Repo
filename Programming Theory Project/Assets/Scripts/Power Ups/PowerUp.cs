using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    /// <summary>
    /// Reference to the renderer so rendering can be stopped.
    /// </summary>
    private Renderer rend;
    // Power Up values
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public float AttackSpeed { get; private set; }
    public ProjectileSpawner.ShotType ShotType { get; private set; }
    
    // Internal logic
    /// <summary>
    /// Time after which the PowerUp is destroyed.
    /// </summary>
    public float LifeTime { get; private set; }
    /// <summary>
    /// How long the PowerUp is active after it is picked up.
    /// </summary>
    public float LifeTimeAfterPickup { get; private set; }
    /// <summary>
    /// Flag if the PowerUp was already picked up.
    /// </summary>
    private bool isPickedUp = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        Initialize();
    }

    /// <summary>
    /// Initializes the PowerUp. To be overridden by children to set their according powers.
    /// </summary>
    virtual public void Initialize()
    {
        Health = 0;
        Damage = 0;
        AttackSpeed = 0;
        ShotType = ProjectileSpawner.ShotType.Single;
        LifeTime = 3.0f;
        LifeTimeAfterPickup = 1.5f;
    }

    private void Update()
    {
        // check if the PowerUp ist out of time and should be destroyed
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isPickedUp && other.gameObject.CompareTag("Player"))
        {
            // set PowerUp as picked up by the player
            isPickedUp = true;
            LifeTime = LifeTimeAfterPickup;
            rend.enabled = false;   // don't show pickup without stopping its internal logic (SetActive(false) would stop Update & Coroutine)
            other.gameObject.GetComponent<PlayerController>().AttachPowerUp(this);
        }
    }
}
