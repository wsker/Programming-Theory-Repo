using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : PowerUp
{
    override public void Initialize()
    {
        base.Initialize();
        Health = 5;
    }
}
