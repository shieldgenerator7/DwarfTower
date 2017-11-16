using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletChecker : NetworkBehaviour
{

    public int damage = 10;//how much damage this bullet does

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject != null && !ReferenceEquals(collider.gameObject, null))
        {
            HealthPool hp = collider.gameObject.GetComponent<HealthPool>();
            if (hp)
            {
                hp.addHealthPoints(-damage);
                CmdDestroy();
            }
        }
    }

    [Command]
    void CmdDestroy()
    {
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }

}
