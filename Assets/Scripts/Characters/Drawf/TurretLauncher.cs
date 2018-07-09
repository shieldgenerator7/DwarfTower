using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretLauncher : PlayerAbility
{
    private Vector2 bc2dOffset;//the offset of the collider

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (isLocalPlayer)
        {
            bc2dOffset = GetComponent<BoxCollider2D>().offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            bool shouldLaunch = Input.GetMouseButtonUp(1);
            if (shouldLaunch)
            {
                //Mana
                if (!mana.hasEnoughMana(manaCost, 0.5f))
                {
                    return;
                }
                mana.useMana(manaCost);
                Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position + bc2dOffset);
                direction.Normalize();
                Vector2 start = (Vector2)transform.position + bc2dOffset + (direction * spawnBuffer);
                CmdLaunch(start);
            }
        }
    }

    [Command]
    void CmdLaunch(Vector2 start)
    {
        //spawn bullet
        GameObject turret = GameObject.Instantiate(spawnObjectPrefab);
        turret.transform.position = start;
        turret.GetComponent<TurretController>().owner = gameObject;
        TeamToken.assignTeam(turret, gameObject);
        NetworkServer.Spawn(turret);
    }
}
