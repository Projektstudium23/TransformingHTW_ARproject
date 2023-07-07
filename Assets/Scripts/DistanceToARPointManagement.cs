using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceToARPointManagement : MonoBehaviour
{
    TextMeshProUGUI distanceVerticalGarden;
    TextMeshProUGUI distanceUrbanGarden;
    TextMeshProUGUI distanceAtrium;
    TextMeshProUGUI distanceTinyForest1;
    TextMeshProUGUI distanceTinyForest2;

    // Start is called before the first frame update
    void Start()
    {
        distanceVerticalGarden = GameObject.FindGameObjectWithTag("DistanceVerticalGarden").GetComponent<TextMeshProUGUI>();
        distanceUrbanGarden = GameObject.FindGameObjectWithTag("DistanceUrbanGarden").GetComponent<TextMeshProUGUI>();
        distanceAtrium = GameObject.FindGameObjectWithTag("DistanceAtrium").GetComponent<TextMeshProUGUI>();
        distanceTinyForest1 = GameObject.FindGameObjectWithTag("DistanceTinyForest1").GetComponent<TextMeshProUGUI>();
        distanceTinyForest2 = GameObject.FindGameObjectWithTag("DistanceTinyForest2").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // entfernung immer updaten, je nach abstand -> methode unten aufrufen
    }

    //methode die GPSManager nach entfernung knackt
}
