﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CaravanController : NetworkBehaviour
{

    public float maxPushRange = 10;//the maximum distance a player can be from this caravan and still push it
    public float maxPushSpeedPerPlayer = 1.5f;
    public float maxSpeed = 4;//the maximum speed the payload can move

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (isServer)
        {
            //Destroy all trees that happen to overlap the payload
            RaycastHit2D[] rch2ds = new RaycastHit2D[100];
            foreach (Collider2D coll in GetComponents<Collider2D>())
            {
                int count = coll.Cast(Vector2.zero, rch2ds);
                for (int i = 0; i < count; i++)
                {
                    HealthPool hp = rch2ds[i].collider.gameObject.GetComponent<HealthPool>();
                    if (hp)
                    {
                        hp.addHealthPoints(-hp.HP);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            float dirForce = 0;
            foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
            {
                float playerPushForce =
                    maxPushSpeedPerPlayer
                    * Mathf.Max(0, maxPushRange - Vector2.Distance(pc.transform.position, transform.position))
                    / maxPushRange;
                dirForce += TeamManager.getForceDirection(pc) * playerPushForce;

            }
            dirForce = Mathf.Sign(dirForce) * Mathf.Clamp(Mathf.Abs(dirForce), 0, maxSpeed);
            rb2d.velocity = Vector2.up * dirForce;
        }
    }
}
