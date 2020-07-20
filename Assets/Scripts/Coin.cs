﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Powerup
{
    // config
    [SerializeField] int value = 1;

    protected override void ApplyPowerup(Player player)
    {
        session.IncreaseScore(value, true);
    }
}
