using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsMarker : MonoBehaviour
{

    RectTransform trans;
    void Start()
    {
        GpsService.Instance.StartStopGPS();
        trans = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hi");
        float x, y;
        (x,y) = GpsService.Instance.GetMapCoordinates();
        //Debug.Log(x+ ", " + y);
        trans.anchoredPosition = new Vector3(x,y, transform.position.z );
    }
}
