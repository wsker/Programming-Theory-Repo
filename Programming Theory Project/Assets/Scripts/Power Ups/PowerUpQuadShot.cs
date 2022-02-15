using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpQuadShot : PowerUp
{
    override public void Initialize()
    {
        base.Initialize();
        ShotType = ProjectileSpawner.ShotType.Quad;
        LifeTimeAfterPickup = 2.5f;
    }
}
