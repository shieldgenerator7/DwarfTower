using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUpdater : MonoBehaviour {

    private SpriteRenderer sr;

    // Use this for initialization
    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        //if the object doesn't move,
        if (GetComponent<Rigidbody2D>() == null)
        {
            //set its display order once
            sr.sortingOrder = LevelManager.getDisplaySortingOrder(transform.position);
            //and then destroy it
            Destroy(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
        sr.sortingOrder = LevelManager.getDisplaySortingOrder(transform.position);
    }
}
