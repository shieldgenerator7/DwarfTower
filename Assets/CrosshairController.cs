using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{

    public GameObject player;
    public Image image;
    public Canvas canvas;

    private ManaPool mana;

    // Use this for initialization
    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void init(GameObject player)
    {
        this.player = player;
        mana = player.GetComponent<ManaPool>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = new Vector2(viewPos.x * canvas.pixelRect.width,
            viewPos.y * canvas.pixelRect.height);

        if (mana)
        {
            float fillAmount = (float)mana.Mana / (float)mana.maxMana;
            image.fillAmount = fillAmount;
            Color c = image.color;
            c.r = c.g = c.b = fillAmount;
            if (mana.Reloading)
            {
                c.a = 0.5f;
            }
            else
            {
                c.a = 1.0f;
            }
            image.color = c;
        }
    }
}
