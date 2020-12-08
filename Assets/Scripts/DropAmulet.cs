using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAmulet : MonoBehaviour
{
    public GameObject amulet;
    private RubyController ruby;
    private int maxAmulet = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Find Ruby
        GameObject result = GameObject.FindWithTag("RubyController");

        //Get Ruby
        if (result != null)
        {
            ruby = result.GetComponent<RubyController>();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        //Check if all robots have been fixed
        if (ruby.currentAmount == 6)
        {
            //Spawn only one amulet between a certain distance
            if (maxAmulet == 1)
            {
                maxAmulet -= 1;
                Vector2 dist = ruby.transform.position - amulet.transform.position;
                Instantiate(amulet, dist, Quaternion.identity);
            }
        }
    }
}
