using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{

    public GameObject player;
    public Image image;
    public Canvas canvas;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = new Vector2(viewPos.x * canvas.pixelRect.width,
            viewPos.y * canvas.pixelRect.height);
    }
}
