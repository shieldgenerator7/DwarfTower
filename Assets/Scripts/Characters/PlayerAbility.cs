using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAbility : NetworkBehaviour
{
    public int manaCost = 10;//how much mana it costs to launch a bullet
    public float spawnBuffer = 1.5f;//how far from the collider's center the object spawns
    public GameObject spawnObjectPrefab;

    protected ManaPool mana;

    // Use this for initialization
    protected virtual void Start () {
        mana = GetComponent<ManaPool>();
    }
}
