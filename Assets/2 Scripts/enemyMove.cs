using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class dieEnemy : MonoBehaviour
{
    int health;

    public GameObject ragDoll;
    public GameObject electrocutedBody;
    public GameObject spark;

    public GameObject onFire;

    int hitCount;

    Vector3 target = new Vector3(0.08f, 13.52f, -8.81f);
    float speed = 3f;
    
    Rigidbody rb;
    float currentTime;
    float startTime;
    bool timing = false;
    float timer = 3f;

    bool isGrounded = true;
    bool instantiable = true;
    bool startEating;
    public float movementSpeed = 3f;
    bool sparkt;

    float timerr = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTime = startTime;
        startEating = false;
        health = 100;
        sparkt = false;
        hitCount = 0;
    }

    void Update()
    {
        if (startEating)
        {

            damaging(GameObject.Find("base temp"));
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (collision.gameObject.CompareTag("obstacle") || collision.gameObject.CompareTag("fall"))
            {
                if (instantiable)
                {
                    instantiable = false;
                    Instantiate(ragDoll, transform.position, transform.rotation);
                }
                if (gameManager != null)
                {
                    gameManager.GetComponent<GameManager2>().decreaseEnemies();
                }
                Destroy(gameObject);
                //give points to players
                
            }
            else if (collision.gameObject.CompareTag("electricWater"))
            {
                if (instantiable)
                {
                    instantiable = false;
                    sparkt = true;
                    Instantiate(electrocutedBody, transform.position, transform.rotation);
                    Instantiate(spark, transform.position, transform.rotation);
                    Debug.Log(spark.name);
                }
                if (gameManager != null)
                {
                    gameManager.GetComponent<GameManager2>().decreaseEnemies();
                }

                Destroy(gameObject);
                //gives points to players
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                //make it do the nom nom
                Animator anim = GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool("isEating", true);
                    //now disable navmesh agent and navigation script
                    NavMeshAgent temp = GetComponent<NavMeshAgent>();
                    temp.speed = 0;
                    startEating = true;
                   // StartCoroutine(damaging(collision.gameObject));
                }
                if (instantiable)
                {
                    instantiable = false;
//                    Instantiate(ragDoll, transform.position, transform.rotation);
                }
            }
            else if (collision.gameObject.CompareTag("bullet"))
            {
                hitCount++;
                if (hitCount == 1)
                {
                    //instantiate fire on enemy here
                    Instantiate(onFire, transform.position, Quaternion.identity, transform);
                    Transform childObject = transform.Find("Cube");
                    if (childObject != null)
                    {
                        Renderer childRenderer = childObject.GetComponent<Renderer>();
                        if (childRenderer != null)
                        {
                            childRenderer.material.color = new Color(131f / 255f, 51f / 255f, 45f / 255f);
                        }
                    }
                }
                else if (hitCount == 2)
                {
                    Instantiate(onFire, transform.position, Quaternion.identity);
                    if (instantiable)
                    {
                        instantiable = false;
                        Instantiate(ragDoll, transform.position, transform.rotation);
                    }
                    if (gameManager != null)
                    {
                        gameManager.GetComponent<GameManager2>().decreaseEnemies();
                    }
                    Destroy(gameObject);
                }
            }

        }
    }

    void damaging(GameObject player)
    {
        //yield return new  WaitForSeconds(0);
        if (player != null)
        {
            baseScript basePlayer = player.GetComponent<baseScript>();
            
            
            if (timerr >= 0)
            {
                timerr -= Time.deltaTime;
            }
            else
            {
                timerr = 1f;
                basePlayer.decreaseHealth();
            }
            
        }
    }

    public void sendDamage(GameObject player)
    {
        if (player != null)
        {
//            BaseScript base = player.GetComponent<BaseScript>();
            baseScript basePlayer = player.GetComponent<baseScript>();
            if (basePlayer != null)
            {
                basePlayer.decreaseHealth();
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("electricWater"))
        {
            GameObject gameManager = GameObject.Find("GameManager");

            if (instantiable)
            {
                instantiable = false;
                Instantiate(electrocutedBody, transform.position, transform.rotation);
                Instantiate(spark, transform.position, transform.rotation);
            }
            if (gameManager != null)
            {
                gameManager.GetComponent<GameManager2>().decreaseEnemies();
            }

            Destroy(gameObject);
            //gives points to players

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("obstacle") || other.gameObject.CompareTag("fall"))
        {
            GameObject gameManager = GameObject.Find("GameManager");

            if (instantiable)
            {
                instantiable = false;
                Instantiate(ragDoll, transform.position, transform.rotation);
            }
            if (gameManager != null)
            {
                gameManager.GetComponent<GameManager2>().decreaseEnemies();
            }

            Destroy(gameObject);

        }
        else if (other.gameObject.CompareTag("electricWater"))
        {
            GameObject gameManager = GameObject.Find("GameManager");

            if (instantiable)
            {
                instantiable = false;
                Instantiate(electrocutedBody, transform.position, transform.rotation);
                Instantiate(spark, transform.position, transform.rotation);
            }
            if (gameManager != null)
            {
                gameManager.GetComponent<GameManager2>().decreaseEnemies();
            }

            Destroy(gameObject);
        }
    }

    public void Die()
    {
        if (instantiable)
        {
            instantiable = false;
            Instantiate(ragDoll, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    public void rotateOccasionally()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0 )
        {
            currentTime = startTime;
            //add rotation code here
            Vector3 direction = target - transform.position;

            // Calculate a step size based on rotation speed and deltaTime
            float singleStep = 1f * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), singleStep);

        }
    }

    private void OnDestroy()
    {
        if (sparkt)
        Instantiate(spark, transform.position, transform.rotation);
    }
}
