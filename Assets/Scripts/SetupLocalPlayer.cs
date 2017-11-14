using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraController>().player = gameObject;
            GetComponent<PlayerController>().enabled = true;
        }
        Destroy(this);
	}
}
