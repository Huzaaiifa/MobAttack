using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            pulsating par = gameObject.GetComponentInParent<pulsating>();
            if (par != null)
            {
                par.cancelPulsate();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }
}
