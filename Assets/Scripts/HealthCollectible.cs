using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D health)
    {
        RubyController controller = health.GetComponent<RubyController>(); //Get the player controller
        
        //Only give health if the player is in the game
        if (controller != null)
        {
            //See if the player's health is lower than the maximum health
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1); //Change health by 1
                Destroy(gameObject); //Destroy health collectible
            }
        }
    }
}
