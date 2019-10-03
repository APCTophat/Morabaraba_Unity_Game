using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightEffect : MonoBehaviour
{
    public Image SmokeScreen;

    bool InvokeBool;


    private void Start()
    {
        SmokeScreen.enabled = false;
        InvokeBool = false;
    }

    private void Update()
    {
        if (!InvokeBool)
        {
            if (SmokeScreen.enabled == true)
            {
                Invoke("DisableSmoke", 3f);
            }
        }
       
    }


    public void DisableSmoke()
    {
      SmokeScreen.enabled = false;
    }
}
