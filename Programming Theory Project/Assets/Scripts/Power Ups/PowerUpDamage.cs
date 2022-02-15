using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDamage : PowerUp
{
    override public void Initialize()
    {
        base.Initialize();
        Damage = 2;
        LifeTimeAfterPickup = 2.5f;
    }
}
