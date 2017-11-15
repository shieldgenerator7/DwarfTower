using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public float detectionRange = 10.0f;//how close enemies must be in order for the turret to activate
    public GameObject owner;//the player who placed the turret

    private List<GameObject> enemies;
    private GunController gunController;

	// Use this for initialization
	void Start () {
        enemies = new List<GameObject>();
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            GameObject go = pc.gameObject;
            if (go != owner)
            {
                enemies.Add(go);
            }
        }
        gunController = GetComponent<GunController>();
	}
	
	// Update is called once per frame
	void Update () {
        GameObject target = null;
        float minSqrDist = detectionRange * detectionRange;
		foreach(GameObject enemy in enemies)
        {
            float sqrDist = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDist <= minSqrDist)
            {
                minSqrDist = sqrDist;
                target = enemy;
            }
        }
        if (target)
        {
            gunController.target = target.transform.position;
        }
	}
}
