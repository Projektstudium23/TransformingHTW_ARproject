using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openURL(string link)
    {
        Application.OpenURL(link);
    }
}
