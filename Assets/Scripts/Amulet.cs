using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Amulet : MonoBehaviour
{
    private HouseKeeper hk;
    private Renderer color; //Get the amulet's renderer
    private float r,g,b; //Store RGB Values
    private Image amuletIcon; //Get amulet icon

    void Start()
    {
       color = GetComponent<Renderer>(); //Get the amulet's renderer    
    
       //Find the Housekeeper
       GameObject houseKeeping = GameObject.FindGameObjectWithTag("Housekeeper");
       
       //Get Housekeeper script
       if (houseKeeping != null)
       {
           hk = houseKeeping.GetComponent<HouseKeeper>();
       }

       //Find the icon image
       GameObject updateIcon = GameObject.FindGameObjectWithTag("Icon");

       //Get the icon image
       if (updateIcon != null)
       {
           amuletIcon = updateIcon.GetComponent<Image>();
       }
    }

    //Randomize colors to get player's attention to collect the amulet
    void Update()
    {
        //Get random RGB values
        r = Random.Range(0.1f, 1.3f);
        g = Random.Range(0.1f, 1.3f);
        b = Random.Range(0.1f, 1.3f);

        //Produce resulting color
        color.material.color = Color.HSVToRGB(r,g,b);
    }

    //Collect amulet
    void OnTriggerEnter2D (Collider2D player)
    {
        if (player.tag == "RubyController")
        {
            player.GetComponent<RubyController>().collectedAmulet = true; //Amulet is collected

            //Make the amulet icon more visible when collected
            Color becomeVisible = amuletIcon.color;
            becomeVisible.a = 255f;
            amuletIcon.color = becomeVisible;

            hk.quest = false; //Finished quest
            gameObject.SetActive(false); //Set the visibility to false as default
            player.GetComponent<RubyController>().PlaySound(player.GetComponent<RubyController>().collectedClip); //Play sound
            Destroy(gameObject); 
        }
    }
}
