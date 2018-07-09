using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPool : NetworkBehaviour {

    public int maxHP = 100;//the max HP this entity has
    public bool destroyOnDeath = true;//destroys this game object when HP reaches 0
    public bool fadeWithHP = true;//make the object fade out as it takes damage
    
    private int healthPoints;//how much HP this entity currently has
    public int HP
    {
        get
        {
            return healthPoints;
        }
        private set
        {
            healthPoints = value;
            healthPoints = Mathf.Clamp(healthPoints, 0, maxHP);
            if (fadeWithHP)
            {
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();
                }
                Color c = sr.color;
                c.a = (float)healthPoints / (float)maxHP;
                sr.color = c; 
            }
        }
    }
    //
    public delegate void OnHealthLost();
    public delegate void OnHealthGained();
    public delegate void OnDeath();
    //
    public OnHealthLost onHealthLost;
    public OnHealthGained onHealthGained;
    public OnDeath onDeath;

    private SpriteRenderer sr;

    private void Start()
    {
        if (destroyOnDeath)
        {
            onDeath += CmdKill;
        }
        HP = maxHP;

        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Call this to damage or heal the entity.
    /// Positive values heal,
    /// Negative values damage
    /// </summary>
    /// <param name="deltaHP"></param>
    public void addHealthPoints(int deltaHP)
    {
        if (deltaHP < 0)
        {
            if (onHealthLost != null)
            {
                onHealthLost();
            }
        }
        if (deltaHP > 0)
        {
            if (onHealthGained != null)
            {
                onHealthGained();
            }
        }

        HP += deltaHP;

        if (HP <= 0)
        {
            if (onDeath != null)
            {
                onDeath();
            }
        }
    }

    [Command]
    void CmdKill()
    {
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
