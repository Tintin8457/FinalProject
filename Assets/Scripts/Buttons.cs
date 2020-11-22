using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private RubyController ruby; //Get Ruby

    // Start is called before the first frame update
    void Start()
    {
        //Find Ruby
        GameObject decide = GameObject.FindWithTag("RubyController");

        //Get Ruby
        if (decide != null)
        {
            ruby = decide.GetComponent<RubyController>();
        }
    }

    //Get more cogs
    public void MoreCogs()
    {
        //Give the player 20 more cogs
        ruby.cogs += 20;
        ruby.cogCounter.text = "Cogs: " + ruby.cogs.ToString();

        //COST: Lower the player health by 2
        ruby.ChangeHealth(-2);

        ruby.choices = false; //Close the buttons
        ruby.canbuy = false; //Stop from overbuying
        ruby.defaultLoss = true; //Display default message when the player has no more chances
    }

    //Renerate the player's health to be completely full
    public void FullHealth()
    {
        //Give 1 health when the player has 4 health remaining
        if (ruby.currentHealth == 4)
        {
            ruby.ChangeHealth(1); //Give 1 health

            //COST: Lose 4 cogs
            ruby.cogs -= 4;
            ruby.cogCounter.text = "Cogs: " + ruby.cogs.ToString();

            ruby.choices = false; //Close the buttons
            ruby.canbuy = false; //Stop from overbuying
            ruby.defaultLoss = true; //Display default message when the player has no more chances
        }

        //Give 2 health when the player has 3 health remaining
        else if (ruby.currentHealth == 3)
        {
            ruby.ChangeHealth(2); //Give 2 health

            //COST: Lose 4 cogs
            ruby.cogs -= 4;
            ruby.cogCounter.text = "Cogs: " + ruby.cogs.ToString();

            ruby.choices = false; //Close the buttons
            ruby.canbuy = false; //Stop from overbuying
            ruby.defaultLoss = true; //Display default message when the player has no more chances
        }

        //Give 3 health when the player has 2 health remaining
        else if (ruby.currentHealth == 2)
        {
            ruby.ChangeHealth(3); //Give 3 health

            //COST: Lose 4 cogs
            ruby.cogs -= 4;
            ruby.cogCounter.text = "Cogs: " + ruby.cogs.ToString();

            ruby.choices = false; //Close the buttons
            ruby.canbuy = false; //Stop from overbuying
            ruby.defaultLoss = true; //Display default message when the player has no more chances
        }

        //Give 4 health when the player has 1 health remaining
        else if (ruby.currentHealth == 1)
        {
            ruby.ChangeHealth(4); //Give 4 health

            //COST: Lose 4 cogs
            ruby.cogs -= 4;
            ruby.cogCounter.text = "Cogs: " + ruby.cogs.ToString();

            ruby.choices = false; //Close the buttons
            ruby.canbuy = false; //Stop from overbuying
            ruby.defaultLoss = true; //Display default message when the player has no more chances
        }
    }

    //Close the buttons
    public void Quit()
    {
        ruby.choices = false;
        ruby.canbuy = false; //Stop from overbuying
        ruby.defaultLoss = true; //Display default message when the player has no more chances
    }
}
