using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class gameManager : MonoBehaviour
{
    public GameObject[] taggedObjects;
    GameObject cam;

    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(0.4f, 37.3f, 32.4f);
        taggedObjects = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject obj in taggedObjects)
        {
            obj.SetActive(false);
        }
    }

    public void gameOver()
    {
        if (cam != null)
        {
            GameObject player = GameObject.Find("canon");

            if (player != null)
            {
                player.SetActive(false);
            }

            Animator canimator = cam.GetComponent<Animator>();
            if (canimator != null)
            {
                canimator.SetTrigger("GameOver");
                Time.timeScale = 0.2f;
                gameOverPanel.SetActive(true);
            }
        }
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void disableObjects()
    {
        foreach (GameObject obj in taggedObjects)
        {
            obj.SetActive(false);
        }
    }

    public void PlayGame()
    {
        Animator canimator = cam.GetComponent<Animator>();
        if (canimator != null)
        {
            canimator.SetTrigger("GameStart");
        }
        foreach (GameObject obj in taggedObjects)
        {
            obj.SetActive(true);
        }
    }
}
