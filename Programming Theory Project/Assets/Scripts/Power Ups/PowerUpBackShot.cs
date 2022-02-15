using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBackShot : PowerUp
{
    override public void Initialize()
    {
        base.Initialize();
        ShotType = ProjectileSpawner.ShotType.Back;
        LifeTimeAfterPickup = 4.5f;
    }
}
