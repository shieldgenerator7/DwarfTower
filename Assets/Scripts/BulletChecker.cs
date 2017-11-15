using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletChecker : NetworkBehaviour {

    public int damage = 10;//how much damage this bullet does
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && !ReferenceEquals(collision.gameObject, null))
        {
            collision.gameObject.GetComponent<HealthPool>().addHealthPoints(-damage);
            CmdDestroy();
        }
    }

    [Command]
    void CmdDestroy()
    {
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }

}
