using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseScore : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            increaseScore(1);
        }
    }

    public void increaseScore(int pointsToAdd)
    {
        {
            score += pointsToAdd;
            scoreText.text = score.ToString();
        }
    }
}
