using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince : MonoBehaviour
{
    Animator princeMove;
    Rigidbody2D princeRun;
    private float princeSpeed = 1.5f;
    int direction = 1;
    public GameObject jail;

    //Access both the housekeeper and ruby scripts
    private HouseKeeper hk;
    private RubyController goRuby;

    // Start is called before the first frame update
    void Start()
    {
        princeMove = GetComponent<Animator>(); //Get animator
        princeRun = GetComponent<Rigidbody2D>(); //Get rigidbody

        //Get and find the housekeeper
        GameObject questCompleted = GameObject.FindGameObjectWithTag("Housekeeper");

        if (questCompleted != null)
        {
            hk = questCompleted.GetComponent<HouseKeeper>();
        }

        //Get and find Ruby
        GameObject queenFound = GameObject.FindGameObjectWithTag("RubyController");

        if (questCompleted != null)
        {
            goRuby = queenFound.GetComponent<RubyController>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Play the result
        if (hk.playResult == true)
        {
            StartCoroutine("Ending");
        }
    }

    //Ending of the game
    IEnumerator Ending()
    {
        //Destroy prison
        yield return new WaitForSeconds(1f);

        if (hk.barricade != null && jail != null)
        {
            Destroy(jail);
            yield return new WaitForSeconds(1f);
            Destroy(hk.barricade);
        }

        //Get rid of the housekeeper
        yield return new WaitForSeconds(0.50f);
        
        if (hk != null)
        {
            Instantiate(hk.vanish, hk.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.60f);
            Destroy(hk.gameObject);
        }

        //The prince becomes excited that it gets to escape
        princeMove.SetBool("IsFree", true);

        //The prince will run to Ruby
        yield return new WaitForSeconds(1f);

        princeMove.SetBool("NowRun", true);
        yield return new WaitForSeconds(0.80f);
        Vector2 move = princeRun.position; //Set movement for the prince
        // move.x = move.x + Time.deltaTime * princeSpeed * direction;
        // princeRun.MovePosition(move);
        float distance = Vector2.Distance(princeRun.position, goRuby.transform.position); //Get distance between ruby and the prince
        //move.x = move.x + Time.deltaTime * princeSpeed * direction;

        //Move the prince within 7 feet or less until it reaches 1.2ft
        if (distance <= 7.0f && distance > 1.2f)
        {
            move.x = move.x + Time.deltaTime * princeSpeed * direction;
            princeRun.MovePosition(move);
            //princeMove.SetBool("NowRun", true);
        }

        //Stop the prince from running when it is near the player
        if (distance <= 1.2f)
        {
            Debug.Log("Dist:" + distance);

            if (transform.position.x >= -6.56574f)
            {
                yield return new WaitForSeconds(0.05f);
            }

            else if (transform.position.x < -6.56574f)
            {
                yield return new WaitForSeconds(0.02f);
            }

            princeMove.SetBool("RubyTime", true);
            yield return new WaitForSeconds(2.40f);
            goRuby.canRestart = true; //Display win message
        }
    }
}
