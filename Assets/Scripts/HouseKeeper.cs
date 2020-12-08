using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HouseKeeper : MonoBehaviour
{
    public float displayTime = 4.0f; //Amount of time to display the character message
    public GameObject keeperBox; //Use for interact
    float timerDisplay; //How long to display the dialog
    private RubyController ruby;
    public TextMeshProUGUI houseText;
    public static int stage;
    public bool quest; //Use to check that the player read the quest
    //public bool endMessage; //Display as default once the player has won
    public bool playResult; //Activate it after the quest has been completed
    public GameObject barricade;
    public GameObject vanish;
    //private Dice charm; //Avoid anything when quest is done
    Animator keepAnim;

    // Start is called before the first frame update
    void Start()
    {
        keepAnim = GetComponent<Animator>(); //Get animator

        quest = false; //Activate it when the player interacts with the housekeeper
        playResult = false;

        //Have the dialog box not show up by default
        keeperBox.SetActive(false);
        timerDisplay = -1.0f;
        houseText.text = ""; //Start up the fun

        //Find Ruby
        GameObject trackRuby = GameObject.FindWithTag("RubyController");

        //Get ruby script
        if (trackRuby != null)
        {
            ruby = trackRuby.GetComponent<RubyController>();
        }

        //Find charm
        // GameObject getCharm = GameObject.FindWithTag("Charm");

        //Get charm script
        // if (getCharm != null)
        // {
        //     charm = getCharm.GetComponent<Dice>();
        // }

        //Keep track of level number
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        {
            stage = 1;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SecondStage"))
        {
            stage = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Display the dialog box
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;

            //Stop displaying the dialog box once it is below 0
            if (timerDisplay < 0)
            {
                keepAnim.SetBool("CanTalk", false); //Stop interact animation
                keeperBox.SetActive(false); //Disable the message box
                ruby.GetComponent<AudioSource>().Stop();

                //Second level
                if (stage == 2)
                {
                    //Check off the quest after first reading it
                    if (ruby.currentAmount <= 5)
                    {
                        quest = true;
                    }

                    //Allow the player to give the housekeeper the amulet
                    else if (ruby.currentAmount == 6)
                    {
                        //Only display end message when the amulet is collected
                        if (quest == false && ruby.collectedAmulet == true)
                        {
                            //Play the ending of the game
                            playResult = true;
                        }
                    }
                }
            }
        }
    }

    //Display the NPC dialog message
    public void DisplayDialog()
    {
        timerDisplay = displayTime;

        ruby.PlaySound(ruby.npc);

        //Display default message
        keepAnim.SetBool("CanTalk", true); //Show interact animation

        //Level 1 text
        if (stage == 1)
        {
            houseText.fontSize = 27;
            houseText.text = "I'm the Housekeeper. Enter this house, you are in for a treat!";
        }
        
        //Level 2 text
        else if (stage == 2)
        {
            //Only display the quest when the player has not fixed all robots
            if (ruby.currentAmount <= 5)
            {
                //Display quest premise when the player first interacts with the housekeeper
                if (quest == false && ruby.collectedAmulet == false) 
                {
                    houseText.text = "Free the prince by fixing all robots and getting an amulet from them!";
                }

                //Display default text to ensure the player meets this goal
                else if (quest == true && ruby.collectedAmulet == false)
                {
                    houseText.text = "Make sure to fix all robots! An amulet will appear after you have fixed them!";
                }
            }
            
            //Display completed quest message only if all robots are fixed and you have the amulet
            else if (ruby.currentAmount == 6)
            {
                //Completed Quest
                if (quest == false && ruby.collectedAmulet == true)
                {
                    ruby.limitPower = true; //Disable powerups
                    houseText.fontSize = 27;
                    houseText.text = "You have fixed all robots and collected the amulet. The prince is free!";
                    ruby.speed = 0.0f; //Disable movement

                    //Disable powerups
                    // if (charm.chance <= 6)
                    // {
                    //     charm.countDown = 0.0f;
                    // }
                }

                //No amulet
                else if (quest == false && ruby.collectedAmulet == false)
                {
                    houseText.fontSize = 32;
                    houseText.text = "Where's my amulet?";
                }
            }
        }

        keeperBox.SetActive(true); //Show the message 
    }
}