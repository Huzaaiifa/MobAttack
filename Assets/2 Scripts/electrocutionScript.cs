using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electrocutionScript : MonoBehaviour
{
    Transform child;
    BoxCollider childColl;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {

            Transform child = transform.Find("electric");
            if (child != null)
            {
                Transform cube = child.transform.Find("Cube");
                if (cube != null)
                {
                    cube.gameObject.tag = "Untagged";
                }
                pulsating par = child.GetComponent<pulsating>();
                if (par != null)
                {
                    par.cancelPulsate();
                }
            }
            child = transform.Find("electric (1)");
            if (child != null)
            {
                Transform cube = child.transform.Find("Cube");
                if (cube != null)
                {
                    cube.gameObject.tag = "Untagged";
                }

                pulsating par = child.GetComponent<pulsating>();
                if (par != null)
                {
                    par.cancelPulsate();
                }
            }

            child = FindChildRecursive(transform, "Cylinder.005");
            if (child != null)
            {
                child.gameObject.AddComponent<Rigidbody>();
                childColl = child.gameObject.AddComponent<BoxCollider>();
                childColl.isTrigger = true;
            }
        }
    }

    Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent == null)
            return null;

        if (parent.name == name)
            return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            Transform found = FindChildRecursive(child, name);
            if (found != null)
                return found;
        }

        return null;
    }
}
