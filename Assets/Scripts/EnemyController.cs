using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Health;
    public float MaxSpeed;
    public float AccelerationRate;

    // Private Variables
    float Speed;
    float DriftFactor;
    GameObject Player;
    Vector2 PlayerDirection;
    Vector2 PreviousPlayerDirection;
    Rigidbody2D rb;
    BoxCollider2D col;

    public GameObject explosion;

    public GameObject fx;

    public Transform lenemi;

    public float timer;

    public float timer01;

    public SpriteRenderer m_SpriteRenderer;

    //public Rigidbody2D rb;

    public bool ou;
        
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        Player = GameObject.FindWithTag("Player");
        DriftFactor = 1;
    }

    void Update()
    {
        //Should I rotate towards Player ?
        PlayerDirection = Player.transform.position - transform.position;
        if(Mathf.Sign(PlayerDirection.x) != Mathf.Sign(PreviousPlayerDirection.x))
        {
            RotateTowardsPlayer();
        }
        PreviousPlayerDirection = PlayerDirection;

        //Go towards Player
        if(timer <= 0)
        {
            rb.velocity = new Vector2(transform.forward.z * DriftFactor * Speed * Time.fixedDeltaTime, rb.velocity.y);
        }

        if(timer01 <= 0)
        {
            m_SpriteRenderer.color = Color.red;

        }

        if (timer >= 0)
        {
            timer = timer - 1 * Time.deltaTime;
        }

        if (timer01 >= 0)
        {
            timer01 = timer01 - 1 * Time.deltaTime;
        }

        //Die
        if (Health <= 0)
        {
            Instantiate(fx, lenemi.transform.position, Quaternion.identity);
            //SOUND DE MORT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


            Destroy(gameObject);
        }

        if(Speed <= 0)
        {
            StartCoroutine(GetToSpeed(MaxSpeed));
        }
        //Debug.Log(Speed);
    }

    public void GetDamage(float dmg)
    {
        Health -= dmg;

        m_SpriteRenderer.color = Color.white;
        if (Health == 1)
        {
            Instantiate(explosion, lenemi.transform.position, Quaternion.identity);
            this.transform.localScale = new Vector3(0.38f, 0.38f, 0.7f);
            //SOUND DE DOULEURE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        }
        if (ou == true)
        {
            rb.velocity = new Vector3(10, 5, 0);
            timer = 0.7f;
            timer01 = 0.085f;

        }
        if (ou == false)
        {
            rb.velocity = new Vector3(-10, 5, 0);
            timer = 0.7f;
            timer01 = 0.085f;

        }
    }

    void RotateTowardsPlayer()
    {
        if (PlayerDirection.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            ou = true;
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            ou = false;
        }
        DriftFactor = -1;
        StartCoroutine(GetToSpeed(0));
    }

    IEnumerator GetToSpeed( float s)
    {
        //Debug.Log(s);
        float baseSpeed = Speed;
        float SignMultiplier = Mathf.Sign(s - Speed);
        for(float f=baseSpeed; f*SignMultiplier<=s; f += AccelerationRate*SignMultiplier)
        {
            Speed = f;
            yield return null;
        }
        DriftFactor = 1;
    }
}
