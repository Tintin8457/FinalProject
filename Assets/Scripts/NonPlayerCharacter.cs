using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f; //Amount of time to display the character message
    public GameObject dialogBox; //Get the dialog box
    public GameObject transitionBox; //Get stage 2 transition box
    float timerDisplay; //How long to display the dialog
    private RubyController ruby;
    public bool transportRuby; //Allow Ruby to go to second stage

    // Start is called before the first frame update
    void Start()
    {
        //Have the dialog boxes not show up by default
        dialogBox.SetActive(false);
        transitionBox.SetActive(false);
        timerDisplay = -1.0f;
        transportRuby = false;

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
                //Disable default message when the player has not fixed all robots
                if (ruby.currentAmount < 6)
                {
                    dialogBox.SetActive(false);
                }
                
                //Disable stage 2 transition message when the player has fixed all robots
                else if (ruby.currentAmount == 6)
                {
                    transitionBox.SetActive(false);
                    transportRuby = true;
                }
            }
        }
    }

    //Display the NPC dialog message
    public void DisplayDialog()
    {
        timerDisplay = displayTime;

        //Display default message when the player has not fixed all robots
        if (ruby.currentAmount < 6)
        {
            dialogBox.SetActive(true);
        }

        //Display stage 2 transition message when the player has fixed all robots
        else if (ruby.currentAmount == 6)
        {
            transitionBox.SetActive(true);
        }
    }
}
