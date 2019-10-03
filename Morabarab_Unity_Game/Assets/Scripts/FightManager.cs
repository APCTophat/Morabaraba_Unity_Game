using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public bool PHASE_2 = false;

    public float Attack_Range;
    public float Defence_Range;

        
    public Button P1_Fighter;
    float P1_HP;
    float P1_Attack;
    float P1_Defence;

    float P1_Attack_Min;
    float P1_Attack_Max;

    float P1_Defence_Min;
    float P1_Defence_Max;

       
    public Button P2_Fighter;
    float P2_HP;
    float P2_Attack;
    float P2_Defence;

    float P2_Attack_Min;
    float P2_Attack_Max;

    float P2_Defence_Min;
    float P2_Defence_Max;



    // Start is called before the first frame update
    void Start()
    {
        if (PHASE_2)
        {
            P1_Attack_Max = P1_Attack + Attack_Range;
            P1_Attack_Min = P1_Attack - Attack_Range;

            P1_Defence_Max = P1_Defence + Defence_Range;
            P1_Defence_Min = P1_Defence - Defence_Range;

            P2_Attack_Max = P2_Attack + Attack_Range;
            P2_Attack_Min = P2_Attack - Attack_Range;

            P2_Defence_Max = P2_Defence + Defence_Range;
            P2_Defence_Min = P2_Defence - Defence_Range;

        }

        P1_HP = 10;
        P1_Defence = 10;
        P1_Attack = 10;
        P2_HP = 10;
        P2_Attack = 10;
        P2_Defence = 10;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (PHASE_2)
        {

        }
       
    }


    


    void CalculateP1Stats()
    {
        P1_Attack = Random.Range(P1_Attack_Min, P1_Attack_Max);
        P1_Defence = Random.Range(P1_Defence_Min, P1_Defence_Max);
    }

    void CalculateP2Stats()
    {
        P2_Attack = Random.Range(P2_Attack_Min, P2_Attack_Max);
        P2_Defence = Random.Range(P2_Defence_Min, P2_Defence_Max);
    }


    void CalulateWinner()
    {
        //int winner = (Random.Range(1, 101));

      

        P1_HP = P1_HP - (P2_Attack - P1_Defence);
        P2_HP = P2_HP - (P1_Attack - P2_Defence);


    }




}
