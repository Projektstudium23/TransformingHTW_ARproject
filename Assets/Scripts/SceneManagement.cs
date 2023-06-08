using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void StartNewScene(string nameOfNewScene)
    {
        try
        {
            SceneManager.LoadScene(nameOfNewScene);
        }
        catch
        {
            Debug.Log("no Such Scene found. Check if spelled correctly and if its added to the build Settings");
        }
    }

}
