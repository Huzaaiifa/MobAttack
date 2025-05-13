using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

public class baseScript : MonoBehaviour
{
    int health;
    public GameObject explosion;
    GameObject cam;
    float shakeDuration = 1.5f;
    float shakeMagnitude = 2f;

    public Image blood1;
    public Image blood2;
    public Image blood3;
    public Image blood4;
    public Image blood5;
    int bloodCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
/*    void Update()
    {
        
    }
*/
/*    private void OnCollisionEnter(Collision collision)
    {
        //enter script for decreasing health
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (health > 0)
            {
                health -= 30;
                //add script for explosion here
                GameObject tempExplosion = Instantiate(explosion, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(tempExplosion, 4f);
                StartCoroutine(camShake());
                bloodCounter++;
                if (bloodCounter == 1)
                {
                    blood1.gameObject.SetActive(true);
                }
                else if (bloodCounter == 2)
                {
                    blood2.gameObject.SetActive(true);
                }
                else if (bloodCounter == 3)
                {
                    blood3.gameObject.SetActive(true);
                }
                else if (bloodCounter == 4)
                {
                    blood4.gameObject.SetActive(true);
                }
                else if (bloodCounter == 5)
                {
                    blood5.gameObject.SetActive(true);
                }

            }
            else
            {

            }
        }
    }
*/
    public void decreaseHealth()
    {
        if (health > 0)
        {
            health -= 30;

            if (health < 80 && health >= 60)
            {
                blood1.gameObject.SetActive(true);
            }
            else if (health < 60 && health >= 40)
            {
                blood2.gameObject.SetActive(true);
            }
            else if (health < 40 && health >= 20)
            {
                blood3.gameObject.SetActive(true);
            }
            else if (health < 20 && health >= 0)
            {
                blood4.gameObject.SetActive(true);
            }

            if (health <= 0)
            {
                blood5.gameObject.SetActive(true);

                Transform[] children = GetComponentsInChildren<Transform>();

                // Iterate through children, destroying each one except the parent
                for (int i = children.Length - 1; i > 0; i--)
                {
                    Destroy(children[i].gameObject);
                }
                //give signal to zombies to start walking??

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                foreach (GameObject enemy in enemies)
                {
                    // Access enemy components or properties here
                    Animator enemator = enemy.GetComponent<Animator>();
                    if (enemator != null)
                    {
                        BoxCollider tempp = GetComponent<BoxCollider>();
                        if (tempp != null)
                        {
                            tempp.isTrigger = true;
                            enemator.SetBool("isWalking", true);
                            NavMeshAgent nav = enemy.GetComponent<NavMeshAgent>();
                            if (nav != null)
                            {
                                nav.speed = 1;
                                GameObject gammang = GameObject.Find("GameManager");
                                if (gammang != null)
                                {
                                    gameManager gameScript = gammang.GetComponent<gameManager>();
                                    if (gameScript != null)
                                    {
                                        gameScript.gameOver();
                                    }
                                }
                            }

                        }
                    }
                }


                // add menu for death screen
            }
        }
    }

    public void explode()
    {
        if (health > 0)
        {
            health -= 1;
            //add script for explosion here
            //use the other prefab too? to blast away the ragdolls? instantiate a box collider here?
            Collider[] colliders = Physics.OverlapSphere(transform.position, 7f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("enemy"))
                {
                    dieEnemy DieEnemy = collider.gameObject.GetComponent<dieEnemy>();
                    if (DieEnemy != null)
                    {
                        DieEnemy.Die();
                    }
                    
                }
            }
            GameObject tempExplosion = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(tempExplosion, 4f);

            Animator temp = cam.GetComponent<Animator>();
            if (temp != null)
            {
                temp.enabled = false;                
            }
            StartCoroutine(camShake());
            if (temp != null)
            {
                temp.enabled = true;
            }

        }
    }

    IEnumerator camShake()
    {
        yield return null;
        float elapsedTime = 0f;
        Vector3 initialPosition = cam.transform.localPosition;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);

            cam.transform.localPosition = initialPosition + new Vector3(x, y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = initialPosition;
    }
}
