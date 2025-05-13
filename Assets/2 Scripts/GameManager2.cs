using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager2 : MonoBehaviour
{
    public GameObject[] taggedObjects;
    GameObject cam;

    public int numberOfEnemies;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(0.4f, 37.3f, 32.4f);
        taggedObjects = GameObject.FindGameObjectsWithTag("enemy");
        numberOfEnemies = taggedObjects.Length;
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

    public void decreaseEnemies()
    {
        numberOfEnemies--;
        if (numberOfEnemies == 0)
        {
            //play Game over menu
            gameWinPanel.SetActive(true);
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

    public void Level1()
    {
        SceneManager.LoadScene("0.1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("0.2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("0.3");
    }
    public void Level4()
    {
        SceneManager.LoadScene("1.1");
    }
    public void Level5()
    {
        SceneManager.LoadScene("1.2");
    }
    public void Level6()
    {
        SceneManager.LoadScene("1.3");
    }

    public bool allEnemiesDead()
    {
        if (numberOfEnemies == 0)
        {
            return true;
        }
        else return false;
    }
}

