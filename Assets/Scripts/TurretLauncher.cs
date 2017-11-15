using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretLauncher : NetworkBehaviour {

    public float launchRate = 6;//how many turrets can be launched in a minute
    public float spawnBuffer = 1f;//how far from the collider's center the bullet spawns

    public GameObject turretPrefab;

    private float launchCoolDownDuration;//how long (in secs) between each shot
    private float nextLaunchTime;//the soonest the next shot can be fired

    private Vector2 bc2dOffset;//the offset of the collider

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            launchCoolDownDuration = 60 / launchRate;
            nextLaunchTime = 0;
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
                if (Time.time >= nextLaunchTime)
                {
                    nextLaunchTime = Time.time + launchCoolDownDuration;
                    Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position + bc2dOffset);
                    direction.Normalize();
                    Vector2 start = (Vector2)transform.position + bc2dOffset + (direction * spawnBuffer);
                    CmdLaunch(start);
                }
            }
        }
    }

    [Command]
    void CmdLaunch(Vector2 start)
    {
        //spawn bullet
        GameObject turret = GameObject.Instantiate(turretPrefab);
        turret.transform.position = start;
        turret.GetComponent<TurretController>().owner = gameObject;
        NetworkServer.Spawn(turret);
    }
}
