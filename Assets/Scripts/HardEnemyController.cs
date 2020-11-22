using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The original EnemyController script is controlled as the default behavior for the HardEnemyController script
public class HardEnemyController : EnemyController
{
    //Remove 2 lives from the player- overrides the CollisionEnter2D method from the original script
    void OnCollisionEnter2D(Collision2D otherTwo)
    {
        if (cantMove.frozen == false)
        {
            RubyController player = otherTwo.gameObject.GetComponent<RubyController>();

            if (player != null)
            {
                player.ChangeHealth(-2);
                Debug.Log("The player lost 2 lives");
            }
        }
    }
}
