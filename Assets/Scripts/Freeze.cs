using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private ActivatedFreeze freezer; //Get the freeze script

    void Start()
    {
        //Find the box controller
        GameObject startFreeze = GameObject.FindGameObjectWithTag("Box"); 

        //Get the freeze script
        if (startFreeze != null)
        {
            freezer = startFreeze.GetComponent<ActivatedFreeze>();
        }
    }

    //Freeze robots for a limited time when the player opens the box
    void OnTriggerEnter2D(Collider2D player)
    {
        RubyController grabRuby = player.GetComponent<RubyController>(); //Get the player script

        //Activate the robot freeze time
        if (grabRuby != null)
        {
            freezer.frozen = true; //start to freeze robots for a limited time
            Destroy(gameObject); //Destroy robots
        }
    }
}
