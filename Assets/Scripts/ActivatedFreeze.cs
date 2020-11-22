using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedFreeze : MonoBehaviour
{
    public bool frozen; //freeze the robots for a limited time
    public float countDown = 20.0f; //Use for timer

    // Start is called before the first frame update
    void Start()
    {
        frozen = false; //Frozen should be deactivated until the player collides with the box
    }

    // Update is called once per frame
    void Update()
    {
        //Start the countdown that freezes the robots for a limited time
        if (frozen == true)
        {
            //Freeze the robots
            countDown -= Time.deltaTime;

            //Unfreeze the robots
            if (countDown < 1)
            {
                frozen = false;
                countDown = 0;
            }
        }
    }
}
