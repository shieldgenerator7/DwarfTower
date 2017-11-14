using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 5.0f;//how fast the player walks

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = (walkSpeed * horizontal * Vector3.right) + (walkSpeed * vertical * Vector3.up);
        rb2d.velocity = direction;
    }
}
