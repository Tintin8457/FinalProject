using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class House : MonoBehaviour
{
    private RubyController moveRuby;
    public CinemachineConfiner confinerStation; //Default cinemachine script settings
    public PolygonCollider2D extraConfiner; //Confiner to change when the player goes to secret room

    //Teleport the player to a secret room
    void OnTriggerEnter2D(Collider2D ruby)
    {
        //Find Ruby
        GameObject teleRuby = GameObject.FindWithTag("RubyController");

        //Get Ruby script
        if (teleRuby != null)
        {
            moveRuby = teleRuby.GetComponent<RubyController>();
        }
        
        //Secret room
        if (moveRuby.GetComponent<Rigidbody2D>().position.y <= 5.22f)
        {
            moveRuby.GetComponent<Rigidbody2D>().position = new Vector2(83.71611f, 6.07f); //Teleport player
            confinerStation.m_BoundingShape2D = extraConfiner; //Switch confiner
            moveRuby.canbuy = true; //Are you ready?

            //Check if the bargain press limit is lower than 2, reset to 2
            if (moveRuby.offerLimit < 2)
            {
                moveRuby.offerLimit = 2;
            }
        }
    }
}
