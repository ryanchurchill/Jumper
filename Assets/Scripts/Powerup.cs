using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // config
    [SerializeField] protected GameSession session;
    [SerializeField] AudioSource pickupSound;

    public void CollectPowerup(Player player)
    {
        ApplyPowerup(player);
        pickupSound.Play();
        Destroy(gameObject);
    }

    protected abstract void ApplyPowerup(Player player);
}
