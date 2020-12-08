using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public int chance; //Ue for randomization
    private RubyController ruby; //Use to access Ruby
    SpriteRenderer changeDieColor; //Get die color
    public bool canUse; //Limit the player to only one use
    public float countDown = 20f; //Use to limit the luck charm time
    public GameObject mainIcon; //Display the powerup here
    public Sprite speed; //Use for increased speed
    public Sprite rapidFire; //Use for rapid fire

    // Start is called before the first frame update
    void Start()
    {
        //Find Ruby
        GameObject makeEasy = GameObject.FindGameObjectWithTag("RubyController");

        //Get Ruby
        if (makeEasy != null)
        {
            ruby = makeEasy.GetComponent<RubyController>();
        }

        //Get sprite renderer of die
        changeDieColor = GetComponent<SpriteRenderer>();

        canUse = true; //The player can collect the luck charm
        mainIcon.SetActive(false); //Disable the powerup icon
    }

    // Update is called once per frame
    void Update()
    {
        if (ruby.limitPower == false)
        {
            //1-3: Increase speed
            if (chance > 0 && chance <= 3)
            {
                ruby.speed = 4.5f;
            }

            //4-6: Rapid fire
            else if (chance >= 4)
            {
                ruby.rapidFire = true;
            }

            //Start the countdown of 20 secs
            if (canUse == false)
            {
                countDown -= Time.deltaTime;

                if (countDown <= 0f)
                {
                    //Stop the countdown by 0
                    countDown = 0.0f;

                    //Reset the player's speed back
                    if (chance <= 3)
                    {
                        ruby.speed = 3f;
                    }

                    //Reset the player's shooting ability to only one cog at a time
                    else if (chance >= 4)
                    {
                        ruby.rapidFire = false;
                    }

                    mainIcon.SetActive(false); //Disable the powerup icon

                    Destroy(gameObject); //Get rid of the luck charm
                }
            }
        }
    }
    
    //3 possible events happen if the player collects a luck charm
    void OnTriggerEnter2D(Collider2D player)
    {
        //Only use the luck charm once
        if (canUse == true)
        {
            if (player.tag == "RubyController")
            {
                //Determine random # between 1 and 6
                chance = Random.Range(1, 7);

                //Change to a different color to inform the player that has been activated and cannot be used more than once!
                Color visualCue = changeDieColor.color;
                
                //Enable the powerup icon
                mainIcon.SetActive(true);

                //Change to red when random # is 1 - 3
                if (chance <= 3)
                {
                    visualCue.g = 0f;
                    visualCue.b = 0f;

                    //Display powerup icon to the speed icon
                    mainIcon.GetComponent<Image>().sprite = speed;
                }

                //Change to yellow when random # is 4 - 6
                else if (chance >= 4)
                {
                    visualCue.b = 0f;

                    //Display powerup icon to the cogs icon
                    mainIcon.GetComponent<Image>().sprite = rapidFire;
                }

                changeDieColor.color = visualCue;

                canUse = false; //Start the timer and stop the player from getting it more than once
                ruby.PlaySound(ruby.collectedClip); //Play sound
            }
        }
    }
}
