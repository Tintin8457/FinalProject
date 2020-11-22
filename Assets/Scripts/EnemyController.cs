using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical; //Check if the enemy is moving vertically
    public float changeTime = 3.0f; //Use for moving back and forth

    Rigidbody2D rigidbody2D;

    //Use for back-forth movement
    float timer; 
    int direction = 1;

    Animator enemyAnimator; 

    public bool broken = true; //Default broken state

    public ParticleSystem smokeEffect;

    private RubyController ruby;

    public AudioSource enemySound;
    public AudioClip walk;
    public AudioClip fix;

    public ActivatedFreeze cantMove; //The robots cannot move when they are frozen
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); //Get rigidbody
        timer = changeTime; //Activate timer
        enemyAnimator = GetComponent<Animator>(); //Get animator

        //Default sound
        enemySound.clip = walk;
        enemySound.Play();

        //Find Ruby
        GameObject myRuby = GameObject.FindWithTag("RubyController");

        //Get ruby script
        if (myRuby != null)
        {
            ruby = myRuby.GetComponent<RubyController>();
        }
    }
    
    void Update()
    {
        if (cantMove.frozen == false)
        {
            //Keep as default: position can move, rotation must be disabled, animations should be enabled!
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            enemyAnimator.enabled = true;
            
            //See if the robot is not broken
            if (!broken)
            {
                return;
            }

            //Move the enemy from the time
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                direction = -direction;
                timer = changeTime;
            }
        }
        
        //Freeze from moving when the player collides with the box
        else if (cantMove.frozen == true)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            enemyAnimator.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (cantMove.frozen == false)
        {
            //See if the robot is not broken
            if (!broken)
            {
                return;
            }

            //Move the enemy
            Vector2 position = rigidbody2D.position;
        
            if (vertical)
            {
                position.y = position.y + Time.deltaTime * speed * direction;

                //Play animations
                enemyAnimator.SetFloat("Move X", 0);
                enemyAnimator.SetFloat("Move Y", direction);
            }

            else
            {
                position.x = position.x + Time.deltaTime * speed * direction;

                //Play animations
                enemyAnimator.SetFloat("Move X", direction);
                enemyAnimator.SetFloat("Move Y", 0);
            }
        
            rigidbody2D.MovePosition(position);
        }
    }

    //Fix the robot
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        enemyAnimator.SetTrigger("Fixed");
        enemySound.clip = fix;
        enemySound.Play();
        smokeEffect.Stop();
        ruby.RobotCounter(1);
    }

    //Remove a life from the player
    void OnCollisionEnter2D(Collision2D other)
    {
        if (cantMove.frozen == false)
        {
            RubyController player = other.gameObject.GetComponent<RubyController>();

            if (player != null)
            {
                player.ChangeHealth(-1);
            }
        }
    }
}
