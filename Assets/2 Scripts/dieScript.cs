using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dieScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        if (gameObject.CompareTag("electrocuted"))
        {
            yield return new WaitForSeconds(6.5f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }
}
