using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waterElectrocution : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Cylinder.005")
        {
            collision.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            Debug.Log(collision.gameObject.name);
            gameObject.tag = "electricWater";
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource != null )
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }


}
