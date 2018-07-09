using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkEffect : MonoBehaviour {

    public float blinkSpeed = 0.2f;//how often it changes its state for the blink effect
    public float minAlpha = 0.1f;//the alpha value of the sprite when it flashes

    public bool Blinking
    {
        get { return activated; }
        set {
            activated = value;
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            setBlinkState(activated);
            lastTransitionTime = Time.time;
        }
    }
    private bool activated = false;
    private float lastTransitionTime;//the point in time of the last transition

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Blinking)
        {
            if (Time.time > lastTransitionTime + blinkSpeed)
            {
                lastTransitionTime = Time.time;
                setBlinkState(sr.color.a == 1);
            }
        }
	}

    void setBlinkState(bool flash)
    {
        Color c = sr.color;
        if (flash)
        {
            c.a = minAlpha;
        }
        else
        {
            c.a = 1;
        }
        sr.color = c;
    }

    public static void blink(GameObject go, bool shouldBlink)
    {
        BlinkEffect be = go.GetComponent<BlinkEffect>();
        if (be == null)
        {
            if (!shouldBlink)
            {
                return;
            }
            be = go.AddComponent<BlinkEffect>();
        }
        if (be.Blinking != shouldBlink)
        {
            be.Blinking = shouldBlink;
            be.enabled = shouldBlink;
        }
    }
}
