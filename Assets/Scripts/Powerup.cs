using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // config
    [SerializeField] protected GameSession session;

    private void Start()
    {
        if (!session)
        {
            session = FindObjectOfType<GameSession>();
        }
    }

    public void CollectPowerup(Player player)
    {
        ApplyPowerup(player);
        Destroy(gameObject);
    }

    protected abstract void ApplyPowerup(Player player);
}
