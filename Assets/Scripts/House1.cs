using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class House1 : MonoBehaviour
{
    private RubyController moveRuby;
    public CinemachineConfiner confinerStation; //Default cinemachine script settings
    public PolygonCollider2D defaultConfiner; //Default confiner

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

        //Go back to the Main level
        if (moveRuby.GetComponent<Rigidbody2D>().position.x >= 73.84f)
        {
            moveRuby.GetComponent<Rigidbody2D>().position = new Vector2(-1.227622f, -1.410743f); //Teleport player
            confinerStation.m_BoundingShape2D = defaultConfiner; //Switch confiner
            moveRuby.defaultLoss = false; //Reset loss message to false if still enabled
            moveRuby.canbuy = false; //go back to normal, nothing to see here
            moveRuby.choices = false; //Do not display buttons once exited the secret room
        }
    }
}
