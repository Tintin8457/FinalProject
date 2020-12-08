using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private ActivatedFreeze freezer; //Get the freeze script
    private float waitFreeze = 0.8f; //Use to wait until after sound plays

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
            grabRuby.PlaySound(grabRuby.freezeBox); //Play sound
            StartCoroutine("CanFreeze"); //Wait to freeze until sound finishes playing
        }
    }

    //Wait 1 second before freezing robots until sound stops
    IEnumerator CanFreeze()
    {
        yield return new WaitForSeconds(waitFreeze);

        freezer.frozen = true; //start to freeze robots for a limited time
        Destroy(gameObject); //Destroy robots
    }
}
