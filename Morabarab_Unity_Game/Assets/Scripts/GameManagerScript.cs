using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int WhoseTurn;    //an int value that is assigned to either player    0 = player 1 and 1 = player 2
    public int TurnCounter;       //counts turns, kinda explicit in the name there

    public int[] MarkedSpaces;   // takes what ever button was presses, identifies which player is active and then marks it accordingly
                                  //achieves this by changing the buttons int value if the value is 0 = P1 and 1 = P2

    public GameObject[] TurnIcons;    //switchs the two icons on and off to indicate who's turn it in

    public Sprite[] PlayerIcons;   // icons for each player         //0 = P1, 1 = P2, 2= none, 3= P1_FIghter, 4= P2_Fighter

    public Button[] MorabarabaSpaces; // keeps track of each button that is linked to a play space

    //variables relating to counting and displaying how many pieces have been placed and how many are on the board
    public int PB_MaxPieces;
    public int PG_MaxPieces;
    public int PB_CurrentPieces;
    public int PG_CurrentPieces;
    public int MaxPieces;
    public Text PB_MaxPieces_Text;
    public Text PG_MaxPieces_Text;
    public Text PB_CurrentPieces_Text;
    public Text PG_CurrentPieces_Text;

    public int NumberOf3InaRow;

    public List<int> theSolutions;
   // public List<int> exceptions = new List<int>();

    public bool[] BoolExceptionArray = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

    public bool Place_Remove;           //if this bool is true = Place, false = Remove


    //////////PHASE_2//////////////////
    public bool Phase_2 = false;

    public bool Player_1_Chosen = false;
    public bool AllFightersChosen = false;
  

    public Button P1_Fighter_Sprite;
    public Button P2_Fighter_Sprite;

    public Image SmokeAndDust;
    public Text WinnerText;
    public Color Orange = new Color(1.0f, 0.64f, 0.0f);

    ////////Calculate fight///////////
    public float Attack_Range;
    public float Defence_Range;

    float Player1_HP;
    float Player1_Attack;
    float Player1_Defence;
    float P1_Attack_Min;
    float P1_Attack_Max;
    float P1_Defence_Min;
    float P1_Defence_Max;

    float Player2_HP;
    float Player2_Attack;
    float Player2_Defence;
    float P2_Attack_Min;
    float P2_Attack_Max;
    float P2_Defence_Min;
    float P2_Defence_Max;


    public class Fighter
    {
        //public Fighter(float x = 0, float y=0, float z=0)
        //{
        //    _Attack = x;
        //    _Defence = y;
        //    _HP = z;
        //}
        public float _Attack;
        public float _Defence;
        public float _HP;
    }
    public GameManagerScript instance;

    

    private void Awake()
    {
       
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            
        }
    }


    void Start()
    {
        NumberOf3InaRow = 0;
        GameSetUp();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void GameSetUp()
    {
        SmokeAndDust.enabled = false;
        WhoseTurn = 0;
        TurnCounter = 0;
        Place_Remove = true;
        TurnIcons[0].SetActive(true);
        TurnIcons[1].SetActive(false);                          //visual indicator of whose tunr it is
        for (int i = 0; i < MorabarabaSpaces.Length; i++)
        {
            MorabarabaSpaces[i].interactable = true;
            MorabarabaSpaces[i].GetComponent<Image>().sprite = PlayerIcons[2];
        }
        for (int i = 0; i < MarkedSpaces.Length; i++)
        {
            MarkedSpaces[i] = -100;   //sets all the spaces to a value that identifies no players has marked it
        }
        PB_MaxPieces = PG_MaxPieces = MaxPieces;
        PB_CurrentPieces = PG_CurrentPieces = 0;
        

    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //checks board state when called
    public void BoardState()
    {
        PB_CurrentPieces = PG_CurrentPieces = 0;
        for (int i = 0; i < MarkedSpaces.Length; i++)                      //checks the the number of pieces that each player controls, then counts them
        {
            if(MarkedSpaces[i] == 1)
            {
                PB_CurrentPieces++;
                PB_CurrentPieces_Text.text = PB_CurrentPieces.ToString();
            }
            else if (MarkedSpaces[i] == 2)
            {
                PG_CurrentPieces++;
                PG_CurrentPieces_Text.text = PG_CurrentPieces.ToString();
            }
        }

        if(PB_MaxPieces <= 0 && PG_MaxPieces <= 0)                         //begins phase 2
        {
            Phase_2 = true;
            for (int i = 0; i < MorabarabaSpaces.Length; i++)
            {
                MorabarabaSpaces[i].interactable = true;
                if(MarkedSpaces[i] == -100)
                {
                    MorabarabaSpaces[i].interactable = false;
                }
            }
        }
        if (Phase_2)
        {
            if (PB_CurrentPieces <= 0)
            {
                WinnerText.text = "Orange Wins";
                WinnerText.color = Color.red;
            }
            else if (PG_CurrentPieces <= 0)
            {
                WinnerText.text = "Blue Wins";
                WinnerText.color = Color.blue;
            }
        }
        PB_CurrentPieces_Text.text = PB_CurrentPieces.ToString();
        PG_CurrentPieces_Text.text = PG_CurrentPieces.ToString();
    }

    //this function is activated by the buttons onClick function
    public void MorabarabaButton(int WhichNumber)       
    {
        
        if (!Phase_2)
        {
            if (Place_Remove)
            {

                MorabarabaSpaces[WhichNumber].image.sprite = PlayerIcons[WhoseTurn];    //turns the pressed button to the player who pressed it icon
                MorabarabaSpaces[WhichNumber].interactable = false;                     //once the button is pressed cannot be pressed again, this is only for certain phases need to set up exceptions

                MarkedSpaces[WhichNumber] = WhoseTurn + 1;                              //identifies what space is marked by who, the +1 if for the checker calculations
                TurnCounter++;


                CheckForThreeInaARow();

                if (Place_Remove)
                {
                    if (WhoseTurn == 0)                                                      //this switches players and indicaes it by activating the sprites
                    {
                        WhoseTurn = 1;
                        TurnIcons[0].SetActive(false);
                        TurnIcons[1].SetActive(true);
                        PB_MaxPieces = PB_MaxPieces - 1;
                        PB_MaxPieces_Text.text = PB_MaxPieces.ToString();
                    }
                    else
                    {
                        WhoseTurn = 0;
                        TurnIcons[0].SetActive(true);
                        TurnIcons[1].SetActive(false);
                        PG_MaxPieces = PG_MaxPieces - 1;
                        PG_MaxPieces_Text.text = PG_MaxPieces.ToString();
                    }
                }


                BoardState();
            }
            else if (!Place_Remove)
            {

                MorabarabaSpaces[WhichNumber].image.sprite = PlayerIcons[2];            //The Icon is set to blank on press

                MarkedSpaces[WhichNumber] = -100;                              //Resets the button's value to -100 to not mess with calculations


                // CheckForThreeInaARow();


                if (WhoseTurn == 0)                                                      //this switches players and indicaes it by activating the sprites
                {
                    WhoseTurn = 1;
                    TurnIcons[0].SetActive(false);
                    TurnIcons[1].SetActive(true);
                    PB_MaxPieces = PB_MaxPieces - 1;
                    PB_MaxPieces_Text.text = PB_MaxPieces.ToString();
                }
                else
                {
                    WhoseTurn = 0;
                    TurnIcons[0].SetActive(true);
                    TurnIcons[1].SetActive(false);
                    PG_MaxPieces = PG_MaxPieces - 1;
                    PG_MaxPieces_Text.text = PG_MaxPieces.ToString();
                }

                PlaceAPiece();
                //CheckForThreeInaARow();      //turn this on if you I want a player to make a 3 in a row on the row a piece was just removed from
                BoardState();
            }
        }
        else if (Phase_2)                   //buttons are chosen to fight
        {
           


            if (!AllFightersChosen)         //to check if two fighters are chosen
            {
              
                if (!Player_1_Chosen)         //if the first one is chose
                {
                    
                 if(MarkedSpaces[WhichNumber] == 1 && WhoseTurn == 0)
                    {
                       // Debug.Log("Picked Blue");
                        WhoseTurn = 1;
                        TurnIcons[0].SetActive(false);
                        TurnIcons[1].SetActive(true);
                        Player_1_Chosen = true;
                        MorabarabaSpaces[WhichNumber].image.sprite = PlayerIcons[2];
                       
                        MarkedSpaces[WhichNumber] = 101;
                        P1_Fighter_Sprite.image.sprite = PlayerIcons[3];

                        Button Fighter_1 = MorabarabaSpaces[WhichNumber];
                        ButtonStatScript P1ButtonValue = Fighter_1.GetComponent<ButtonStatScript>();
                        Fighter P1 = new Fighter
                        {
                            _Attack = P1ButtonValue.AttackDamage,
                            _Defence = P1ButtonValue.DamageMitigation,
                            _HP = P1ButtonValue.HealthPoints
                        };

                        Player1_Attack = P1._Attack;
                        Player1_Defence = P1._Defence;
                        Player1_HP = P1._HP;
                        P1_Attack_Max = P1._Attack + Attack_Range;
                        P1_Attack_Min = P1._Attack - Attack_Range;
                        P1_Defence_Max = P1._Defence + Defence_Range;
                        P1_Defence_Min = P1._Defence - Defence_Range;

                     

                     
                    }
                   
                }
                else if (Player_1_Chosen)
                {
                   // Debug.Log("Player 1 has been chosen");
                    if(MarkedSpaces[WhichNumber] == 2 && WhoseTurn == 1)
                    {
                       // Debug.Log("Picked Orange");
                        WhoseTurn = 0;
                        TurnIcons[0].SetActive(true);
                        TurnIcons[1].SetActive(false);

                        MorabarabaSpaces[WhichNumber].image.sprite = PlayerIcons[2];
                        
                        MarkedSpaces[WhichNumber] = 102;
                        P2_Fighter_Sprite.image.sprite = PlayerIcons[4];

                        Button Fighter_2 = MorabarabaSpaces[WhichNumber];
                        ButtonStatScript P2ButtonValue = Fighter_2.GetComponent<ButtonStatScript>();
                        Fighter P2 = new Fighter
                        {
                            _Attack = P2ButtonValue.AttackDamage,
                            _Defence = P2ButtonValue.DamageMitigation,
                            _HP = P2ButtonValue.HealthPoints
                        };

                        Player2_Attack = P2._Attack;
                        Player2_Defence = P2._Defence;
                        Player2_HP = P2._HP;
                        P2_Attack_Max = P2._Attack + Attack_Range;
                        P2_Attack_Min = P2._Attack - Attack_Range;
                        P2_Defence_Max = P2._Defence + Defence_Range;
                        P2_Defence_Min = P2._Defence - Defence_Range;

                        AllFightersChosen = true;
                       

                    }
                }
            }
        }
    }


    void CheckForThreeInaARow()
    {
       
        int S1 = MarkedSpaces[0] + MarkedSpaces[1] + MarkedSpaces[2];
        int S2 = MarkedSpaces[0] + MarkedSpaces[3] + MarkedSpaces[5];
        int S3 = MarkedSpaces[0] + MarkedSpaces[8] + MarkedSpaces[16];
        int S4 = MarkedSpaces[1] + MarkedSpaces[9] + MarkedSpaces[17];
        int S5 = MarkedSpaces[2] + MarkedSpaces[10] + MarkedSpaces[18];
        int S6 = MarkedSpaces[2] + MarkedSpaces[4] + MarkedSpaces[7];

        int S7 = MarkedSpaces[4] + MarkedSpaces[12] + MarkedSpaces[20];
        int S8 = MarkedSpaces[5] + MarkedSpaces[6] + MarkedSpaces[7];
        int S9 = MarkedSpaces[5] + MarkedSpaces[13] + MarkedSpaces[21];
        int S10 = MarkedSpaces[6] + MarkedSpaces[14] + MarkedSpaces[22];
        int S11 = MarkedSpaces[7] + MarkedSpaces[15] + MarkedSpaces[23];
        int S12 = MarkedSpaces[3] + MarkedSpaces[11] + MarkedSpaces[19];         //starts from outer rings

        int S13 = MarkedSpaces[13] + MarkedSpaces[14] + MarkedSpaces[15];
        int S14 = MarkedSpaces[13] + MarkedSpaces[11] + MarkedSpaces[8];
        int S15 = MarkedSpaces[15] + MarkedSpaces[12] + MarkedSpaces[10];
        int S16 = MarkedSpaces[8] + MarkedSpaces[9] + MarkedSpaces[10];       //starting in middle row

        int S17 = MarkedSpaces[21] + MarkedSpaces[22] + MarkedSpaces[23];
        int S18 = MarkedSpaces[21] + MarkedSpaces[19] + MarkedSpaces[16];
        int S19 = MarkedSpaces[16] + MarkedSpaces[17] + MarkedSpaces[18];
        int S20 = MarkedSpaces[23] + MarkedSpaces[20] + MarkedSpaces[18];     //starting in inner row

        theSolutions = new List<int>(new int[] { S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14, S15, S16, S17, S18, S19, S20 });
        
        for (int i = 0; i < theSolutions.Count; i++)
        {

          
            if (theSolutions[i] == 3 * (WhoseTurn + 1)) //this functions checks the int value for the solutions, formula (3 * (0+1)) or (3 * (1+1))
                                                        // since X = 1 ( 1 + 1 + 1 = 3) so if S1 int value = 3 
                                                        // since O = 2 (2 + 2 + 2 = 6) so if S1 int vlaue = 6 
            {
              

                if (BoolExceptionArray[i] == false)
                {
                    NumberOf3InaRow++;
                    Debug.Log(NumberOf3InaRow);
                    BoolExceptionArray[i] = true;
                    RemoveAPiece();
                }
               
                
                
            }
            else if (theSolutions[i] != 3 && theSolutions[i] != 6)
            {
                if (BoolExceptionArray[i] == true)
                {
                    BoolExceptionArray[i] = false;
                }
            }
        }

    } 

    public void RemoveAPiece()
    {
        Place_Remove = false;
        for (int i = 0; i < MarkedSpaces.Length; i++)
        {
            if(WhoseTurn == 0)      //if player one made the 3 in a row
            {
                if(MarkedSpaces[i] == 2)                       //takes the buttons of the opposite player and makes them interactable
                {
                    MorabarabaSpaces[i].interactable = true;
                    
                }
                if(MarkedSpaces[i] == -100)                    //takes all the blank spaces and makes then non-interactabe, so players can't remove nothing   
                {
                    MorabarabaSpaces[i].interactable = false;
                }
            }
            if (WhoseTurn == 1)      //if player two made the 3 in a row
            {
                if (MarkedSpaces[i] == 1)
                {
                    MorabarabaSpaces[i].interactable = true;
                   
                }
                if (MarkedSpaces[i] == -100)                    //takes all the blank spaces and makes then non-interactabe, so players can't remove nothing   
                {
                    MorabarabaSpaces[i].interactable = false;
                }
            }
        }

       
    }

    public void PlaceAPiece()
    {
        Place_Remove = true;
       
        for (int i = 0; i < MarkedSpaces.Length; i++)
        {
            MorabarabaSpaces[i].interactable = false;
            if (MarkedSpaces[i] == -100)
            {
                MorabarabaSpaces[i].interactable = true;
            }
        }
    }

    /////////PHASE 2 VOIDS////////
     
    public void StartFightVisuals()
    {
       
        if (Phase_2)
        {
            if (AllFightersChosen)
            {
                SmokeAndDust.enabled = true;
                Invoke("CalculateFight", 1f);
            }
              
        }
       
    }

    public void CalculateFight()
    {
       
        Player1_Attack = Random.Range(P1_Attack_Min, P1_Attack_Max);
        Player1_Defence = Random.Range(P1_Defence_Min, P1_Defence_Max);
        Player2_Attack = Random.Range(P2_Attack_Min, P2_Attack_Max);
        Player2_Defence = Random.Range(P2_Defence_Min, P2_Defence_Max);


        Player1_HP = Player1_HP - (Player2_Attack - Player1_Defence);
        Player2_HP = Player2_HP - (Player1_Attack - Player2_Defence);
        

        if(Player1_HP <= Player2_HP)
        {
            if(Player1_HP <= 0)       //player 2 wins
            {
                WinnerText.text = "Orange Won";
                WinnerText.color = Color.red;
                Invoke("ResetForNextFight", 1f);
                Invoke("OrangeWon", 1f);
            }
            else 
            {
                CalculateFight();
            }
        }
        else if(Player2_HP <= Player1_HP)
        {
            if (Player2_HP <= 0)      //player 1 wins
            {
                WinnerText.text = "Blue Won";
                WinnerText.color = Color.blue;
                Invoke("ResetForNextFight", 1f);
                Invoke("BlueWon", 1f);
                

            }
            else
            {
                CalculateFight();
            }
        }
    }

   public void BlueWon()
    {
       
        P1_Fighter_Sprite.image.sprite = PlayerIcons[2];
        P2_Fighter_Sprite.image.sprite = PlayerIcons[2];
       

        for (int i = 0; i < MarkedSpaces.Length; i++)
        {
            if(MarkedSpaces[i] == 101)
            {

                MarkedSpaces[i] = 1;
                MorabarabaSpaces[i].image.sprite = PlayerIcons[0];
            }

            if(MarkedSpaces[i] == 102)
            {
                MarkedSpaces[i] = -100;
            }
           
        }
       

    }

    public void OrangeWon()
    {
       
        P2_Fighter_Sprite.image.sprite = PlayerIcons[2];
        P1_Fighter_Sprite.image.sprite = PlayerIcons[2];
      

        for (int i = 0; i < MarkedSpaces.Length; i++)
        {
            if (MarkedSpaces[i] == 101)
            {
                MarkedSpaces[i] = -100;
            }

            if (MarkedSpaces[i] == 102)
            {

                MarkedSpaces[i] = 2;
                MorabarabaSpaces[i].image.sprite = PlayerIcons[1];
               
            }

        }
    }

    public void ResetForNextFight()
    {
       
        SmokeAndDust.enabled = false;
        WinnerText.text = " ";

        AllFightersChosen = false;
        Player_1_Chosen = false;
        Debug.Log("reset");
        BoardState();
    }

    
}
