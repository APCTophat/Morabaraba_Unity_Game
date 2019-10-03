using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStatScript : MonoBehaviour
{
    //Determin what type of button this is
    public bool Attack;
    public bool Defense;
    public bool HP;

    //the 'buttons' stats that are set in game will be the same for every button
    public float AttackDamage;
    public float DamageMitigation;
    public float HealthPoints;

    public float MultiplicationValue;



    void Start()
    {
        SetUpStats();
    }

   
    void Update()
    {
        
    }


    void SetUpStats()
    {
        if (Attack)
        {
            AttackDamage = AttackDamage * MultiplicationValue;
        }
        else if (Defense)
        {
            DamageMitigation = DamageMitigation * MultiplicationValue;
        }
        else if (HP)
        {
            HealthPoints = HealthPoints * MultiplicationValue;
        }
    }

}
