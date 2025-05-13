using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Ground")) //CHECK THIS LATER BECAUSE GROUND AND GLASS NEED TO BE DIFFERENT TAGS I THINK?
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene("Menu");
        Destroy(gameObject);
    }
}
