using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f; //Current speed that the player can move
    public int maxHealth = 5;
    public float timeInvincible = 2.0f; //Default amount of time to be invincible

    public int health { get { return currentHealth; }} //Get the health
    public int currentHealth; //Use for current health

    //Check for invincible and set a timer for it
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D playerRB; //Get playerRB
    
    //Sore movement input data
    float horizontal;
    float vertical;

    Animator playerAnimator;
    Vector2 lookDirection = new Vector2(1,0); //Direction that the player looks at

    public GameObject projectilePrefab; //Get projectile

    AudioSource audioSource; //Player audio source

    //Audioclips
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip collectedClip;

    //Particle System Prefabs
    public GameObject hitEffect;
    public GameObject pickupEffect;

    //Text component
    public TextMeshProUGUI robotCount;
    public TextMeshProUGUI outcomeText;
    public TextMeshProUGUI cogCounter;

    public int currentAmount = 0; //Score

    //Display either winText or loseText
    //public GameObject winBox;
    public GameObject outcomeBox;

    public bool popUp;
    private NonPlayerCharacter jambi;
    
    public static int level;
    public bool playWinMusic;

    public int cogs; //Fire a cog if the player has 1 cog
    public int shootLimit = 1; //The player should should only one cog at a time
    public bool canShoot; //Allow the player to shoot
    public bool canbuy; //You will see what happens- allow the player to buy in secret room
    public bool moreCogs; //the player can purchase more cogs
    public bool moreHealth; //the player can gain more health
    public bool choices; //choices will appear
    public int offerLimit = 2; //Limit the player from presssing the X button many times
    public bool defaultLoss; //Display default message when the player has no more choices
    // public AudioSource gamePlayer;
    // public AudioClip defaultMusic;
    // public AudioClip loseMusic;
    // public AudioClip winMusic;

    // Start is called before the first frame update
    void Start()
    {
       playerAnimator = GetComponent<Animator>(); //Get animator
       playerRB = GetComponent<Rigidbody2D>(); //Get player rigidbody
       currentHealth = maxHealth; //Start off with 5 lives
       audioSource = GetComponent<AudioSource>(); //Get AudioSource
       robotCount.text = "Robots Fixed: " + currentAmount.ToString(); //Display fixed robots on text
       outcomeText.text = ""; //Display outcome
    //    winBox.SetActive(false); //Win box should be turned off
       outcomeBox.SetActive(false); //Outcome box should be turned off
       popUp = true; //Toggled when the player wins stage 1
       cogs = 6; //Set the cog amount to 6   
       cogCounter.text = "Cogs: " + cogs.ToString(); //Display the amount of cogs
       canbuy = false; //it's a surprise
       moreCogs = false; //you'll see
       moreHealth = false; //wait until you get inside the house!
       choices = false; //I highly suggest to go inside the house to find out for yourself
       defaultLoss = false; //This will happen
       canShoot = true; //Allow the player to shoot
    
       //Find NPC script
       GameObject enableMove = GameObject.FindGameObjectWithTag("Jambi");
       
       //Get NPC script
       if (enableMove != null)
       {
           jambi = enableMove.GetComponent<NonPlayerCharacter>();
       }

       //Set level to 1
       if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
       {
           level = 1;
           playWinMusic = false;
       }

       //Set level to 2
       if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SecondStage"))
       {
           level = 2;
           playWinMusic = true;
       }

       Debug.Log("Current level: " + level);

       //Keep updated score and health in the second stage
    //    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SecondStage"))
    //    {
    //         robotCount.text = "Robots Fixed: " + PlayerPrefs.GetInt("currentAmount").ToString();
    //    }

       //Play the default game music
    //    gamePlayer.clip = defaultMusic;
    //    gamePlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 0)
        {
            //Get keyboard input
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical"); 

            Vector2 move = new Vector2(horizontal, vertical);

            //Check if the player is moving
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            //Play animations based on facing direction and speed
            playerAnimator.SetFloat("Look X", lookDirection.x);
            playerAnimator.SetFloat("Look Y", lookDirection.y);
            playerAnimator.SetFloat("Speed", move.magnitude);

            //Check for player invincibility
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;

                if (invincibleTimer < 0)
                {
                    isInvincible = false;
                }
            }
        
            //Launch the projectile
            if(Input.GetKeyDown(KeyCode.C))
            {
                Launch();
            }

            //Interact with the NPC
            if(Input.GetKeyDown(KeyCode.X))
            {
                //Use a raycast to see if the player is facing the NPC
                RaycastHit2D hit = Physics2D.Raycast(playerRB.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

                //Test if the raycast is directly aiming at the NPC
                if (hit.collider != null)
                {
                    //Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                
                    //Get the npc to hit it with a raycast
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                    //Display dialog box
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }

                    //Transport player to the second stage
                    if (currentAmount == 6 && jambi.transportRuby == true)
                    {
                        SceneManager.LoadScene("SecondStage");
                    }

                    //Get the Bargainer to hit it with a raycast
                    Bargainer bargain = hit.collider.GetComponent<Bargainer>();

                    //Display dialog box
                    if (bargain != null && offerLimit <= 2)
                    {
                        //The player can choose once
                        if (offerLimit == 2 && defaultLoss == false)
                        {
                            offerLimit -= 1; //Decrease to zero so the player does not press many times
                            bargain.DisplayDialog();
                        }

                        //Display default message once the player has no more chances
                        else if (offerLimit == 1 && defaultLoss == true)
                        {
                            offerLimit -= 1; //Decrease to zero so the player does not press many times
                            bargain.DisplayDialog();
                        }
                    }
                }
            }

            //Reset the shoot limit if the projectile has been destroyed
            if (shootLimit == 0 && canShoot == true)
            {
                shootLimit += 1;
            }
        }

        //Escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //Diplay a lose message when the player has 0 lives remaining
        if (currentHealth == 0)
        {
            //Play lose music
            // gamePlayer.clip = loseMusic;
            // gamePlayer.Play();

            outcomeBox.SetActive(true); //Display lose text
            outcomeText.text = "Game Over! Game by Brandon Perez. Press R to restart."; //Display lose text
            Restart();
        }

        //Display the second stage message when all robots have been fixed
        if (currentAmount == 6)
        {
            //Play win music
            // gamePlayer.clip = winMusic;
            // gamePlayer.Play();

            //Store current robots fixed to appear in the second stage
            //PlayerPrefs.SetInt("currentAmount", currentAmount);
            
            //Level 1!
            if (level == 1)
            {
                if (popUp == true)
                {
                    speed = 0.0f; //Disable movement
                    outcomeBox.SetActive(true); //Display stage 2 text
                    outcomeText.text = "Talk to Jambi to proceed to Stage 2! Press Q to close!"; //Display win text
                }

                //Close the popup by pressing Q
                if (Input.GetKey(KeyCode.Q))
                {
                    popUp = false;
                    speed = 3.0f;
                    outcomeBox.SetActive(false); //Disable Outcome box
                }
            }
            
            //Level 2!
            if (level == 2)
            {
                //Display the win message when all robots have been fixed
                speed = 0.0f; //Disable movement
                outcomeBox.SetActive(true); //Display win text
                outcomeText.text = "You have fixed the robots. Game by Brandon Perez. Press R to restart the game or esc to quit the game."; //Display win text
                Restart();
            }
        }
    }

    void FixedUpdate()
    {
        if (currentHealth > 0)
        {
            //Move the player
            Vector2 position = playerRB.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;
            playerRB.MovePosition(position);
        }
    }

    //Play sounds
    public void PlaySound(AudioClip clip)
    {
        if (currentHealth > 0)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    //Change health
    public void ChangeHealth (int amount)
    {
        if (currentHealth > 0)
        {
            if (amount < 0)
            {
                playerAnimator.SetTrigger("Hit"); //Play hit animation

                //Cannot be hurt from invincibility?
                if (isInvincible)
                {
                    return;
                }
            
                isInvincible = true;
                invincibleTimer = timeInvincible;

                //Display hit effect particles
                Instantiate(hitEffect, playerRB.position + Vector2.up * 0.5f, Quaternion.identity);
                PlaySound(hitSound);
            }

            //Display pickup effect particles
            if (amount > 0)
            {
                //Spawn pickup particles when the player collects a health pickup
                Instantiate(pickupEffect, playerRB.position + Vector2.up * 0.5f, Quaternion.identity);
                PlaySound(collectedClip); //Play sound
            }

            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            //Debug.Log(currentHealth + "/" + maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
    }

    //Fix robots
    void Launch()
    {
        //The player will only shoot projectiles until they have no projectiles left
        if (currentHealth > 0 && cogs > 0 && shootLimit == 1 && canShoot == true)
        {
            shootLimit -= 1; //Stop the shooting more than once

            //Spawn a new projectile
            GameObject projectileObject = Instantiate(projectilePrefab, playerRB.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>(); //Get projectile capability
            projectile.Launch(lookDirection, 300); //Launch the projectile
            cogs -= 1; //Cog count gets decreased by 1
            cogCounter.text = "Cogs: " + cogs.ToString(); //Display the current amount of cogs
            playerAnimator.SetTrigger("Launch"); //Play launch animation
            PlaySound(throwSound);
            canShoot = false; //Stop from shooting
        }
    }
    
    //Track the fixed Robots
    public void RobotCounter(int robot)
    {
        if (currentHealth > 0)
        {
            currentAmount += robot; //Track amount of robots fixed
            robotCount.text = "Robots Fixed: " + currentAmount.ToString(); //Display # of fixed robots

            //Update score from total robots fixed in level 1
            // if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SecondStage"))
            // {
            //     currentAmount = PlayerPrefs.GetInt("currentAmount");
            //     currentAmount += robot;
            //     robotCount.text = "Robots Fixed: " + currentAmount.ToString();
            //     Debug.Log("New score " + currentAmount);
            // }
        }
    }

    //Change the amount of ammos
    public void CogsIncrement(int cogNum)
    {
        cogs += cogNum; //Increase the amount of cogs
        cogCounter.text = "Cogs: " + cogs.ToString(); //Display the new amount of cogs
        PlaySound(collectedClip);
    }

    //Restart the game when the player wins or lose
    void Restart()
    {
        speed = 0.0f; //Disable movement

        //Restart the game
        if(Input.GetKeyDown(KeyCode.R))
        {
            //Restart the current level by lives loss
            if (currentHealth == 0)
            {
                //Restart the first stage while in level 1
                if (level == 1)
                {
                    SceneManager.LoadScene("MainScene");
                }
            
                //Restart the second stage while in level 2
                if (level == 2)
                {
                    SceneManager.LoadScene("SecondStage");
                }
            }

            //Restart the game when the player is on the second stage and has 6 points
            if (level == 2 && currentAmount == 6)
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    //Play hit effect after colliding into the enemy
    // void OnCollisionEnter2D(Collision2D enemy)
    // {
    //     if (enemy.gameObject.tag == "Enemy")
    //     {
    //         //Hit particle effect only plays if the particle system is not looping and Stop Action is to Destroy
    //         if (hitEffect.main.loop == false && hitEffect.main.stopAction == ParticleSystemStopAction.Destroy)
    //         {
    //             Instantiate(hitEffect, playerRB.position, Quaternion.identity);
    //             hitEffect.Play();
    //             Debug.Log("Particle Effect: " + hitEffect);
    //         }
    //         // Instantiate(hitEffect);
    //         // hitEffect.Play();
    //     }
    // }
}
