﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public float walkSpeed = 5.0f;//how fast the player walks

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraController>().player = gameObject;
            rb2d = GetComponent<Rigidbody2D>();
        }
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            setupForNewPlayer(pc);
        }
    }

    void setupForNewPlayer(PlayerController pc)
    {
        TeamToken tt = pc.gameObject.GetComponent<TeamToken>();
        if (pc.isLocalPlayer)
        {
            tt.teamColor = Color.white;
        }
        tt.teamCaptain = pc.gameObject;
        pc.gameObject.GetComponent<SpriteRenderer>().color = tt.teamColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0 || vertical != 0)
            {
                float terrainMultiplier = LevelManager.getTile(transform.position).terrainSpeedMultiplier;
                Vector2 direction = (terrainMultiplier * walkSpeed * horizontal * Vector3.right) + (terrainMultiplier * walkSpeed * vertical * Vector3.up);
                rb2d.velocity = direction;
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }
        }
    }
}
