using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakSideGlass : MonoBehaviour
{
    public float shatterForce = 10f;
    public GameObject fallDetector;

    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.parent != null)
                {
                    child.parent.GetComponent<BoxCollider>().enabled = false;
                }
                child.parent = null;

                Rigidbody rb = child.GetComponent<Rigidbody>();
                Vector3 forceDirection = 0.5f * (child.transform.position - collision.gameObject.transform.position).normalized;

                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                Collider collider = child.GetComponent<Collider>();
                if (collider != null)
                {
                    Destroy(collider);
                }

            }
            Instantiate(fallDetector, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
        }
    }
}
