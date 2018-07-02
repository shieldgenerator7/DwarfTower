using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public GameObject owner;//the player who placed the turret
    private Vector2 direction;

    private GunController gunController;

    // Use this for initialization
    void Start()
    {
        gunController = GetComponent<GunController>();
    }

    public void playerFired(Vector2 start, Vector2 target)
    {
        gunController.target = target;
    }
}
