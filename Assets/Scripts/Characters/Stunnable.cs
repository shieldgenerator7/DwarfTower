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
    public float postStunInvulnerabeDuration = 1;//how many seconds of invulnerability you get after being stunned
    
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
    }

    private void Update()
    {
        if (Stunned)
        {
            stunDuration = Mathf.MoveTowards(stunDuration, stunDuration * (1 - stunResistancePercent), Time.deltaTime);
            rb2d.velocity = knockbackDirection * (knockbackSpeed * (1 - knockbackResistancePercent));
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

    public delegate void OnStunned(float duration, float knockbackSpeed);
}
