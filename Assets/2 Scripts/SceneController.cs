using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene 2");
    }
    
}