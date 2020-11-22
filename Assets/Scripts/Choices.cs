using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choices : MonoBehaviour
{
    private RubyController ruby; //Ruby the better
    public GameObject quit;
    public GameObject moreAmmos;
    public GameObject moreHealth;

    // Start is called before the first frame update
    void Start()
    {
        //Find Ruby
        GameObject barg = GameObject.FindWithTag("RubyController");

        //Get Ruby
        if (barg != null)
        {
            ruby = barg.GetComponent<RubyController>();
        }
    }

    // Update is called once per frame
    //Activate or deactivate buttons
    void Update()
    {
        //Check if the choices are ready to be made and enable them
        //For more cogs
        if (ruby.choices == true && ruby.moreCogs == true)
        {
            quit.SetActive(true);
            moreAmmos.SetActive(true);
        }

        //For more health
        else if (ruby.choices == true && ruby.moreHealth == true)
        {
            quit.SetActive(true);
            moreHealth.SetActive(true);
        }

        //Disable them when the player pressess a button
        //cogs
        else if (ruby.choices == false && ruby.moreCogs == true)
        {
            quit.SetActive(false);
            moreAmmos.SetActive(false);
            ruby.moreCogs = false;
        }

        //health
        else if (ruby.choices == false && ruby.moreHealth == true)
        {
            quit.SetActive(false);
            moreHealth.SetActive(false);
            ruby.moreHealth = false;
        }
    }
}
