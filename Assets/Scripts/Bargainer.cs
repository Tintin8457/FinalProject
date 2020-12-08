using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bargainer : MonoBehaviour
{
    public float displayTime = 4.0f; //Amount of time to display the character message
    public GameObject sellerBox; //Use for bargon
    float timerDisplay; //How long to display the dialog
    private RubyController ruby;
    public TextMeshProUGUI bargainText;
    private int choose; //Decide

    // Start is called before the first frame update
    void Start()
    {
        //Have the dialog box not show up by default
        sellerBox.SetActive(false);
        timerDisplay = -1.0f;
        bargainText.text = ""; //Start up the fun

        //Find Ruby
        GameObject sixRobots = GameObject.FindWithTag("RubyController");

        //Get ruby script
        if (sixRobots != null)
        {
            ruby = sixRobots.GetComponent<RubyController>();
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
                ruby.GetComponent<AudioSource>().Stop();

                //Disable default message when the player has not fixed all robots
                if (ruby.canbuy == false)
                {
                    sellerBox.SetActive(false);
                    Debug.Log("Go away!");

                    //Stop displaying default message when the player has no more chances
                    ruby.defaultLoss = false;
                }
                
                //The fun ends!
                else if (ruby.canbuy == true)
                {
                   sellerBox.SetActive(false); //Stop the bargain

                   //Display the options if the player meets the offer from either choice
                   if (choose == 1 && ruby.currentHealth > 2)
                   {
                        ruby.choices = true;
                        ruby.moreCogs = true;
                   }

                   else if (choose == 2 && ruby.currentHealth < 5 && ruby.cogs >= 4)
                   {
                        ruby.choices = true;
                        ruby.moreHealth = true;
                   }

                   //Stop trying again to get a different offer if failed
                   else if (choose == 1 && ruby.currentHealth < 3)
                   {
                        ruby.canbuy = false; //Stop from trying again
                        ruby.defaultLoss = true; //Display default message when the player has no more chances
                   }

                   else if (choose == 2 && ruby.currentHealth == 5)
                   {
                        if (ruby.cogs > 3)
                        {
                            ruby.canbuy = false; //Stop from trying again
                        }

                        else if (ruby.cogs < 4)
                        {
                            ruby.canbuy = false; //Stop from trying again
                        }

                        ruby.defaultLoss = true; //Display default message when the player has no more chances
                    }

                    else if (choose == 2 && ruby.currentHealth < 5 && ruby.cogs < 4)
                    {
                        ruby.canbuy = false; //Stop from trying again
                        ruby.defaultLoss = true; //Display default message when the player has no more chances
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

        //Display default message if the player cannot buy anything and the limit is 0
        if (ruby.canbuy == false && ruby.offerLimit == 0 && ruby.defaultLoss == true)
        {
            bargainText.text = "Scram! The shop is closed!";
            sellerBox.SetActive(true); //Show the bargain 
            Debug.Log("Loser");
        }

        //The fun begins- bargain time
        else if (ruby.canbuy == true && ruby.offerLimit == 1 && ruby.defaultLoss == false)
        {
            choose = Random.Range(1,3); //Choose a num between 1 and 2
            sellerBox.SetActive(true); //Show the bargain 

            //Check #1- Offer 20 cogs for 2 lives
            //Check if the player can be offered this deal if their health is greater than 2!
            if (choose == 1 && ruby.currentHealth > 2)
            {
                bargainText.text = "Do you want 20 extra cogs?! If you want more cogs, give me 2 of your lives!";
                ruby.moreCogs = true; //Display interaction with more cogs
                Debug.Log("Choice: 1- default");
            }

            //Check #2- Offer full health for 4 cogs! 
            //Check if the currentHealth is lower than 5 and that the player has 4 or more cogs to be offered this deal!
            else if (choose == 2 && ruby.currentHealth < 5 && ruby.cogs >= 4)
            {
                bargainText.text = "Do you want an instant health regeneration?! Give me 4 of your cogs!";
                ruby.moreHealth = true; //wait until you get inside the house!
                Debug.Log("Choice: 2- default");
            }

            //Display default message when the player does not match up for any offer for choice 1
            else if (choose == 1 && ruby.currentHealth < 3)
            {
                bargainText.text = "Sorry, Chubby Ruby! Come back after you play with your Rubik's Cube!";
                Debug.Log("Choice: 1- backup");
            }

            //Display default messages when the player does not match up for any offer for choice 2
            else if (choose == 2 && ruby.currentHealth == 5)
            {
                //Display message with the player has more than 3 cogs
                if (ruby.cogs > 3)
                {
                    bargainText.text = "Get lost, loser! Don't come back until you get some exercise!";
                    Debug.Log("Choice: 2- backup #1");
                }

                //Display message if the player has less than 4 cogs
                else if (ruby.cogs < 4)
                {
                    bargainText.text = "Get more booty so you I can feast on more flies!";
                    Debug.Log("Choice: 2- backup #2");
                }
            }

            else if (choose == 2 && ruby.currentHealth < 5 && ruby.cogs < 4)
            {
                bargainText.text = "I will feed you to the alligators if you don't get me booty!";
                Debug.Log("Choice: 2- backup #3");
            }
        }
    }
}
