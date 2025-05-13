using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logDisappear : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (audioSource != null )
            {
                audioSource.Play();
                gameObject.tag = "Untagged";
//                audioSource.PlayOneShot(audioSource.clip, 0.5f);
            }
            Destroy(gameObject, 2.5f);
        }
    }
}
