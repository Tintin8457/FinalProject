using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float displayTime = 4.0f; //Amount of time to display the character message
    public GameObject plantBox; //Use for interact
    float timerDisplay; //How long to display the dialog
    private RubyController ruby;
    Animator plantAnim;
    private float deathWait = 0.2f; //Use to wait before displaying death animation
    private float destroyWait = 2.8f; //Use to wait until the death animation is finished

    // Start is called before the first frame update
    void Start()
    {
        plantAnim = GetComponent<Animator>(); //Get animator

        //Have the dialog box not show up by default
        plantBox.SetActive(false);
        timerDisplay = -1.0f;

        //Find Ruby
        GameObject trackRuby = GameObject.FindWithTag("RubyController");

        //Get ruby script
        if (trackRuby != null)
        {
            ruby = trackRuby.GetComponent<RubyController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Display the dialog box
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;

            //Stop displaying the dialog box once it is below 0
            if (timerDisplay < 0)
            {
                plantBox.SetActive(false); //Disable the message box
                StartCoroutine("WaitForDeath"); //Destroy the plant
            }
        }
    }

    //Display the NPC dialog message
    public void DisplayDialog()
    {
        timerDisplay = displayTime;

        //Play sound
        ruby.PlaySound(ruby.npc);

        //Show interact animation
        plantAnim.SetBool("Showcase", true);
        plantAnim.SetBool("Done", false);

        //Show the message 
        plantBox.SetActive(true);
    }

    //Wait a little bit before the plant's death
    IEnumerator WaitForDeath()
    {
        //Display death animation
        yield return new WaitForSeconds(deathWait);
        
        plantAnim.SetBool("Showcase", false);
        plantAnim.SetBool("Done", true);

        //Destroy the plant
        yield return new WaitForSeconds(destroyWait);

        ruby.GetComponent<AudioSource>().Stop();
        Destroy(gameObject);
    }
}
