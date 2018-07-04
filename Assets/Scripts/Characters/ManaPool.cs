using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class ManaPool : MonoBehaviour
{

    public int maxMana = 100;
    public float reloadDuration = 3;//how long reload lasts

    [SerializeField]
    private int mana;
    public int Mana
    {
        get { return mana; }
        set { mana = Mathf.Clamp(value, 0, maxMana); }
    }

    private float reloadStartTime = 0;
    public bool Reloading
    {
        get { return reloadStartTime > 0 && Time.time < reloadStartTime + reloadDuration; }
        private set { }
    }

    private void Start()
    {
        Mana = maxMana;
    }

    private void Update()
    {
        if (Reloading)
        {
            Mana = (int)(maxMana * (Time.time - reloadStartTime) / reloadDuration);
        }
        else if (reloadStartTime > 0 && Time.time - Time.deltaTime < reloadStartTime + reloadDuration)
        {
            Mana = maxMana;
            reloadStartTime = 0;
        }
        else if (Mana <= 0)
        {
            reloadStartTime = Time.time;
        }
    }

    /// <summary>
    /// Checks to see if there's enough mana for the given wanted amount 
    /// </summary>
    /// <param name="requestedAmount">How much mana to request</param>
    /// <param name="percentage">A number from 0 to 1, how much of the requested amount is absolutely necessary</param>
    /// <returns></returns>
    public bool hasEnoughMana(int requestedAmount, float percentage = 1)
    {
        return !Reloading && Mana >= requestedAmount * percentage;
    }

    public int useMana(int requestedAmount)
    {
        if (Reloading)
        {
            return 0;
        }
        if (Mana < requestedAmount)
        {
            Mana -= requestedAmount;
            return Mana;
        }
        else
        {
            Mana -= requestedAmount;
            return requestedAmount;
        }
    }
}
