using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ARButtonManagement : MonoBehaviour
{
    TextMeshProUGUI distanceVerticalGarden;
    TextMeshProUGUI distanceUrbanGarden;
    TextMeshProUGUI distanceAtrium;
    TextMeshProUGUI distanceTinyForest1;
    TextMeshProUGUI distanceTinyForest2;

    Button activateVerticalGarden;
    Button activateUrbanGarden;
    Button activateAtrium;
    Button activateTinyForest1;
    Button activateTinyForest2;
    int verticalGardenID = 1;
    int urbanGardenID = 2;
    int atriumID = 3;
    int tinyForest1 = 4;
    int tinyForest2 = 5;

    bool updateDistance = false;
    int currentIDtoCheck = 0;
    // Start is called before the first frame update
    void Start()
    {
        activateVerticalGarden = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();
        activateUrbanGarden = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();
        activateAtrium = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();
        activateTinyForest1 = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();
        activateTinyForest2 = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();

        distanceVerticalGarden = GameObject.FindGameObjectWithTag("DistanceVerticalGarden").GetComponent<TextMeshProUGUI>();
        distanceUrbanGarden = GameObject.FindGameObjectWithTag("DistanceUrbanGarden").GetComponent<TextMeshProUGUI>();
        distanceAtrium = GameObject.FindGameObjectWithTag("DistanceAtrium").GetComponent<TextMeshProUGUI>();
        distanceTinyForest1 = GameObject.FindGameObjectWithTag("DistanceTinyForest1").GetComponent<TextMeshProUGUI>();
        distanceTinyForest2 = GameObject.FindGameObjectWithTag("DistanceTinyForest2").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateDistance)
        {
            checkForSpecificLocationThroughID(currentIDtoCheck);
        }
    }

    public void ARcanvasActivated(int id) {
        currentIDtoCheck = id;
        updateDistance = true;
    }

    public void ARCanvasDeactivated()
    {
        updateDistance = false;
        currentIDtoCheck = 0;
    }

    // auf übereinstimmende location checken, um zugehörigen initializeAR Button zu aktivieren
    void checkForSpecificLocationThroughID(int id)
    {
        if (GPSService.Instance.StartStopGPS())
        {
            Location currentLocation = GPSService.Instance.GetCurrentLocation();
            if (currentLocation.id == id)
            {
                switch (id)
                {
                    case 1:
                        activateVerticalGarden.interactable = true;
                        break;
                    case 2:
                        activateUrbanGarden.interactable = true;
                        break;
                    case 3:
                        activateAtrium.interactable = true;
                        break;
                    case 4:
                        activateTinyForest1.interactable = true;
                        break;
                    case 5:
                        activateTinyForest2.interactable = true;
                        break;
                }
            }
            else
            {
                float distanceToDisplay = GPSService.Instance.GetDistanceToPointWithID(id);
                string textToDisplay = "Du bist noch " + distanceToDisplay + "m vom Einstiegspunkt entfernt";
                switch (id)
                {
                    case 1:
                        distanceVerticalGarden.text = textToDisplay;
                        break;
                    case 2:
                        distanceUrbanGarden.text = textToDisplay;
                        break;
                    case 3:
                        distanceAtrium.text = textToDisplay;
                        break;
                    case 4:
                        distanceTinyForest1.text = textToDisplay;
                        break;
                    case 5:
                        distanceTinyForest2.text = textToDisplay;
                        break;
                }
            }
        }
    }
}
