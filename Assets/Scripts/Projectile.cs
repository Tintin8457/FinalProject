using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D projectileRB;
    private RubyController ruby;

    // Start is called before the first frame update
    void Awake()
    {
        projectileRB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Find Ruby
        GameObject rubier = GameObject.FindWithTag("RubyController");

        //Get Ruby
        if (rubier != null)
        {
            ruby = rubier.GetComponent<RubyController>();
        }
    }

    //Shoot the projectile
    public void Launch(Vector2 direction, float force)
    {
        projectileRB.AddForce(direction * force);
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy the projectile after a certain distance
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    //Destroy projectile after collision
    void OnCollisionEnter2D(Collision2D other)
    {
        //Fix the enemy
        EnemyController enem = other.collider.GetComponent<EnemyController>();

        if (enem != null)
        {
            enem.Fix();
        }

        Destroy(gameObject);
    }

    //Allow the player to shoot again
    void OnDestroy()
    {
        ruby.canShoot = true;
    }
}
