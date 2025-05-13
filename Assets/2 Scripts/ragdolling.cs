using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdolling : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject mainRig;
    public Animator mainAnimator;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidbodies;


    float timer = 2f;
    bool temp = false;

    // Start is called before the first frame update
    void Start()
    {
        mainRig = gameObject;
//        mainCollider = GetComponent<BoxCollider>();
        mainAnimator = GetComponent<Animator>();
        GetChildrenComponents();
        RagdollON();
    }

    // Update is called once per frame

    public void RagdollON()
    {
        temp = true;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rig in ragdollRigidbodies)
        {
            rig.isKinematic = false;
        }
//        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        mainAnimator.enabled = false;

    }

    public void RagdollOFF()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rig in ragdollRigidbodies)
        {
            rig.isKinematic = true;
        }
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        mainAnimator.enabled = true;
    }


    public void GetChildrenComponents()
    {
        ragdollColliders = mainRig.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = mainRig.GetComponentsInChildren<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            RagdollON();
        }
    }
}
