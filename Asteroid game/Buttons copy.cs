using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");

    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Solar system");//call the main scene solar system 
    }

    public void BackGame()
    {
        SceneManager.LoadScene("Menu");

    }

}