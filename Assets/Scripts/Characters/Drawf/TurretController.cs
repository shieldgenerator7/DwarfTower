using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(GunController))]
public class TurretController : NetworkBehaviour
{
    [SyncVar]
    public GameObject owner;//the player who placed the turret

    private GunController gunController;

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            gunController = GetComponent<GunController>();
            owner.GetComponent<GunController>().EventOnFire += playerFired;
        }
    }

    public void playerFired(Vector2 start, Vector2 target)
    {
        gunController.target = target;
    }
}
