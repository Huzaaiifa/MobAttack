using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Vector3 force;
    public float xDrag = 0f;
    public float yDrag = 0f;
    public float multiplier = 1f; // Ensure this matches forceMultiplier in shoot script
    public Rigidbody rb;
    public GameObject Hit_vfx;
    AudioSource Hit_sfx;

    public bool last = false;
    void Start()
    {
        Hit_sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // Calculate the force using the same method as in the shoot script
        float zDrag = Mathf.Sqrt(Mathf.Pow(xDrag, 2) + Mathf.Pow(yDrag, 2));
        Vector3 launchForce = new Vector3(-xDrag, yDrag * 0.75f, zDrag) * multiplier;
        rb.AddForce(launchForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hit_sfx.PlayOneShot(Hit_sfx.clip, 0.5f);
        Instantiate(Hit_vfx, transform.position, Quaternion.identity);

        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.8f);
        GameObject manager = GameObject.Find("GameManager");
        if (manager != null )
        {
            //check if all objects destroyed //DIS THE CASE IF GAME WON IN LAST BULLET
            if (last && manager.GetComponent<GameManager2>().allEnemiesDead())
            {
                //victory scene
                manager.GetComponent<GameManager2>().gameOver();
            }
            //GET BULLETS HERE AND COMPARE THE BULLETS, IF NONE THEN GAME OVER,ELSE CONTINUE THE GAME //DIS DA CASE WHERE ENEMY NOT DEAD BUT BULLETS FINISH
            else if (last && !manager.GetComponent<GameManager2>().allEnemiesDead())
            {
                manager.GetComponent<GameManager2>().gameOver();
            }
        }
        Destroy(gameObject);

    }

}
