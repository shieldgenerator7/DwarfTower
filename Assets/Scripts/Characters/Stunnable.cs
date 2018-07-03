using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Stunnable : NetworkBehaviour
{
    [Range(0, 1)]
    public float stunResistancePercent = 0;
    [Range(0, 1)]
    public float knockbackResistancePercent = 0;
    [Range(0,10)]
    public float postStunInvulnerabeDuration = 1;//how many seconds of invulnerability you get after being stunned

    public float StunResistance
    {
        get { return stunResistancePercent + stunResistanceBonus; }
        private set { }
    }
    public float KnockbackResistance
    {
        get { return knockbackResistancePercent + knockbackResistanceBonus; }
        private set { }
    }

    [Header("Adaptability")]
    [Range(0, 1)]
    public float stunResistanceIncrement = 0.1f;
    [Range(0, 1)]
    public float knockbackResistanceIncrement = 0.1f;
    [Header("Adaptability Decay")]
    [Range(0, 1)]
    public float stunResistanceDecayPerSecond = 0.01f;
    [Range(0, 1)]
    public float knockbackResistanceDecayPerSecond = 0.01f;
    
    [SyncVar]
    public float stunResistanceBonus = 0;
    [SyncVar]
    public float knockbackResistanceBonus = 0;

    public bool Stunned
    {
        get { return Time.time < stunStartTime + stunDuration; }
        private set { }
    }

    public bool CanBeStunned
    {
        get { return Time.time > stunStartTime + stunDuration + postStunInvulnerabeDuration; }
        private set { }
    }

    private float stunStartTime = 0;
    private float stunDuration = 0;
    private float knockbackSpeed = 0;
    private Vector2 knockbackDirection;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        EventOnUnstunned += adapt;
    }

    private void Update()
    {
        if (Stunned)
        {
            stunDuration = Mathf.MoveTowards(stunDuration, stunDuration * (1 - StunResistance), Time.deltaTime);
            rb2d.velocity = knockbackDirection * (knockbackSpeed * (1 - KnockbackResistance));
        }
        //if just came out of stunned
        else if (Time.time - Time.deltaTime < stunStartTime + stunDuration)
        {
            CmdUnstun();
        }
        //just regular walking around, not stunned
        else
        {
            stunResistanceBonus -= stunResistanceDecayPerSecond * Time.deltaTime;
            stunResistanceBonus = Mathf.Clamp(stunResistanceBonus, 0, 1);
            knockbackResistanceBonus -= knockbackResistanceDecayPerSecond * Time.deltaTime;
            knockbackResistanceBonus = Mathf.Clamp(knockbackResistanceBonus, 0, 1);
        }
    }

    [Command]
    public void CmdStun(float duration, float knockbackSpeed)
    {
        if (CanBeStunned)
        {
            stunStartTime = Time.time;
            stunDuration = duration;
            this.knockbackSpeed = knockbackSpeed;
            this.knockbackDirection = Vector2.down.normalized;
            if (EventOnStunned != null)
            {
                EventOnStunned(duration, knockbackSpeed);
            }
        }
    }

    [Command]
    public void CmdUnstun()
    {
        if (EventOnUnstunned != null)
        {
            EventOnUnstunned();
        }
    }

    /// <summary>
    /// When coming out of stun, gain stun resistance
    /// </summary>
    public void adapt()
    {
        stunResistanceBonus += stunResistanceIncrement;
        knockbackResistanceBonus += knockbackResistanceIncrement;
    }

    public delegate void OnStunned(float duration, float knockbackSpeed);
    [SyncEvent]
    public event OnStunned EventOnStunned;

    public delegate void OnUnstunned();
    [SyncEvent]
    public  event OnUnstunned EventOnUnstunned;
}
