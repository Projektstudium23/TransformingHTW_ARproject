using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsMarker : MonoBehaviour
{
    void Start()
    {
        GpsService.Instance.StartStopGPS();
    }
    // Update is called once per frame
    void Update()
    {
        float x, y;
        (x,y) = GpsService.Instance.GetMapCoordinates();
        transform.position = new Vector3(x,y, transform.position.z );
    }
}
