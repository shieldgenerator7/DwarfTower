using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagChecker : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            //2018-07-08: copied from CaravanController.Start()
            //Destroy all trees that happen to overlap the flag
            RaycastHit2D[] rch2ds = new RaycastHit2D[100];
            foreach (Collider2D coll in GetComponents<Collider2D>())
            {
                int count = coll.Cast(Vector2.zero, rch2ds);
                for (int i = 0; i < count; i++)
                {
                    HealthPool hp = rch2ds[i].collider.gameObject.GetComponent<HealthPool>();
                    if (hp)
                    {
                        hp.addHealthPoints(-hp.HP);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            //Check to see if the caravan entered it
            CaravanController caravan = collision.gameObject.GetComponent<CaravanController>();
            if (caravan)
            {
                FlagChecker enemyFlag = null;
                foreach (FlagChecker fc in FindObjectsOfType<FlagChecker>())
                {
                    if (fc != this)
                    {
                        enemyFlag = fc;
                        break;
                    }
                }
                TeamToken.assignTeam(gameObject, enemyFlag.gameObject);
                Destroy(this);
            }
        }
    }
}
