using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletChecker : NetworkBehaviour
{

    public int damage = 10;//how much damage this bullet does
    public float travelSpeed = 7;//how fast this bullet travels
    public float stunDuration = 2;//how long players will be stunned for when hit
    public float knockbackSpeed = 1;//how fast players move away from the objective when hit

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isServer)
        {
            return;
        }
        if (collider.gameObject != null && !ReferenceEquals(collider.gameObject, null))
        {
            HealthPool hp = collider.gameObject.GetComponent<HealthPool>();
            if (hp)
            {
                if (!TeamToken.isFriendly(gameObject, collider.gameObject))
                {
                    hp.addHealthPoints(-damage);
                    Destroy();
                }
            }
            Stunnable stunnable = collider.gameObject.GetComponent<Stunnable>();
            if (stunnable)
            {
                if (!TeamToken.isFriendly(gameObject, collider.gameObject))
                {
                    stunnable.RpcStun(stunDuration, knockbackSpeed);
                    Destroy();
                }
            }
        }
    }
    
    void Destroy()
    {
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }

}
