using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogsPickup : MonoBehaviour
{
    //Give the player 4 cogs when the player collects the cog pickup object
    void OnTriggerEnter2D (Collider2D moreCogs)
    {
        //Get the ruby script
        RubyController ruby = moreCogs.GetComponent<RubyController>();

        //Check if the player is in the game
        if (ruby != null)
        {
            ruby.CogsIncrement(4); //Increase the cog count by 4
            Destroy(gameObject); //Destroy the pickup once it has been collected
        }
    }
}
