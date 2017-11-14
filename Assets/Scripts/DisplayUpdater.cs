using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUpdater : MonoBehaviour {

    private SpriteRenderer sr;

    // Use this for initialization
    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        sr.sortingOrder = LevelManager.getDisplaySortingOrder(transform.position);
    }
}
