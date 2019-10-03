using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int WhoTurn; // 0 = x and 1 = o
    public int TurnCount; //counts the number of turns played
    public GameObject[] turnIcons; // displays whos turn it is
    public Sprite[] playerIcons; // 0 = x icon and 1 = o icon
    public Button[] tictactoeSpaces; // playable space for game
    public int[] markedSpaces; //ID's which space was marked by which player;

    public int xPlayerScore;
    public int oPlayerScore;
    public Text xPlayerScoreText;
    public Text oPlayerScoreText;


    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        WhoTurn = 0;
        TurnCount = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);                          //visual indicator of whose tunr it is
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < markedSpaces.Length; i++)
        {
            markedSpaces[i] = -100;   //sets all the spaces to a value that identifies no players has marked it
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TicTacToeButton(int WhichNumber)
    {
        tictactoeSpaces[WhichNumber].image.sprite = playerIcons[WhoTurn];
        tictactoeSpaces[WhichNumber].interactable = false; // once a button is clicked it cant be clicked again

        markedSpaces[WhichNumber] = WhoTurn+1; //ID what space is marked by who
        TurnCount++;
        if(TurnCount > 4) // checks if a winner starting from turn 5
        {
            WinnerCheck();
        }

        if (WhoTurn == 0)
        {
            WhoTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
        else
        {
            WhoTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
          
    }

    void WinnerCheck() //runs through each 8 possible solutions to win to see if a player has won
    {
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];         //horizontal solutions

        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];         //vertical soloutions

        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];         //diagonal solutions

        var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };
        for (int i = 0; i < solutions.Length; i++)
        {
            if(solutions[i] == 3 * (WhoTurn + 1)) //this functions checks the int value for the solutions, formula (3 * (0+1)) or (3 * (1+1))
                                                  // since X = 1 ( 1 + 1 + 1 = 3) so if S1 int value = 3 X wins
                                                  // since O = 2 (2 + 2 + 2 = 6) so if S1 int vlaue = 6 0 wins
            {
                WinnerDisplay(i); // I equals the winner
                return;
            }
        }
    }

    void WinnerDisplay(int indexIn)
    {
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = false; // game is over no more interacting with buttons
            //can use a panel to cover over all buttons to make them non-interactable
        }

        if(WhoTurn == 0)
        {
            xPlayerScore++;
            xPlayerScoreText.text = xPlayerScore.ToString();
        }
        else if (WhoTurn == 1)
        {
            oPlayerScore++;
            oPlayerScoreText.text = oPlayerScore.ToString();
        }
    }
}
