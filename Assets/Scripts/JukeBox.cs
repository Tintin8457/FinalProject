using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    public AudioSource gamePlayer;
    // public AudioClip defaultMusic;
    // public AudioClip loseMusic;
    // public AudioClip winMusic;
    private RubyController changeMusic;
    public AudioSource loseMusic;
    public AudioSource winMusic;

    // Start is called before the first frame update
    void Start()
    {
        //Find Ruby
        GameObject musicSwitch = GameObject.FindWithTag("RubyController");

        //Get ruby script
        if (musicSwitch != null)
        {
            changeMusic = musicSwitch.GetComponent<RubyController>();
        }
        
        // // //Play the default game music
        // gamePlayer.clip = defaultMusic;
        // gamePlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // //Play lose music
        // if (changeMusic.currentHealth == 0)
        // {
        //     gamePlayer.clip = loseMusic;
        //     gamePlayer.Play();
        //     Debug.Log("Hello there");
        // }

        // //Play win music
        // if (changeMusic.currentAmount == 6)
        // {
        //     gamePlayer.clip = winMusic;
        //     gamePlayer.Play();
        //     Debug.Log("Hello there");
        // }

        //Play lose music
        if (changeMusic.currentHealth == 0)
        {
            gamePlayer.enabled = false;
            loseMusic.enabled = true;
        }

        //Play win music
        if (changeMusic.playWinMusic == true && changeMusic.currentAmount == 6)
        {
            gamePlayer.enabled = false;
            winMusic.enabled = true;
        }
    }
}
