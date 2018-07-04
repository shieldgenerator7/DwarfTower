using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(TeamToken))]
public class GunController : NetworkBehaviour
{

    public bool automatic = false;//true: auto-fires, false: fires on player input
    public float fireRate = 60;//how many bullets can be fired in a minute
    public float spawnBuffer = 1.5f;//how far from the collider's center the bullet spawns
    public int manaCost = 10;//how much mana it costs to launch a bullet
    [SyncVar]
    public Vector2 target;//the world space coordinate of the target

    public GameObject bulletPrefab;

    private float fireCoolDownDuration;//how long (in secs) between each shot
    private float nextFireTime;//the soonest the next shot can be fired

    private Vector2 bc2dOffset;//the offset of the collider

    private ManaPool mana;

    // Use this for initialization
    void Start()
    {
        if (automatic || isLocalPlayer)
        {
            fireCoolDownDuration = 60 / fireRate;
            nextFireTime = 0;
            bc2dOffset = GetComponent<BoxCollider2D>().offset;
            mana = GetComponent<ManaPool>();
        }
        target = transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if ((automatic && isServer) || isLocalPlayer)
        {
            bool shouldFire = automatic || Input.GetMouseButton(0);
            if (shouldFire)
            {
                if (Time.time >= nextFireTime)
                {
                    if (!automatic)
                    {
                        //Mana
                        int manaGiven = mana.useMana(manaCost);
                        if (manaGiven == 0)
                        {
                            return;
                        }
                        //Target
                        target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                    nextFireTime = Time.time + fireCoolDownDuration;
                    Vector2 direction = target - ((Vector2)transform.position + bc2dOffset);
                    direction.Normalize();
                    Vector2 start = (Vector2)transform.position + bc2dOffset + (direction * spawnBuffer);
                    CmdFire(start, direction, target);
                }
            }
        }
    }

    [Command]
    void CmdFire(Vector2 start, Vector2 direction, Vector2 target)
    {
        //spawn bullet
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = start;
        bullet.GetComponent<Rigidbody2D>().velocity =
            direction.normalized * bullet.GetComponent<BulletChecker>().travelSpeed;
        TeamToken.assignTeam(bullet, gameObject);
        NetworkServer.Spawn(bullet);
        if (EventOnFire != null)
        {
            EventOnFire(start, target);
        }
    }

    //2018-07-01: got help from https://docs.unity3d.com/ScriptReference/Networking.SyncEventAttribute.html
    public delegate void OnFire(Vector2 start, Vector2 target);
    [SyncEvent]
    public event OnFire EventOnFire;
}
