using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseApp : MonoBehaviour
{
    //only works in build
    public void closeApp()
    {
        Debug.Log("Clicked on Exit Button.");
        Application.Quit();
    }
  


}
