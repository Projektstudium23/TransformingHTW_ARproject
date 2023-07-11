using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ARButtonManagement : MonoBehaviour
{
	TextMeshProUGUI distanceText;

	Button activateVerticalGarden;
	Button activateUrbanGarden;
	Button activateAtrium;
	Button activateTinyForest1;
	Button activateTinyForest2;

	//IDs given for GPS coordinates
	int verticalGardenID = 1;
	int urbanGardenID = 2;
	int atriumID = 3;
	int tinyForest1 = 4;
	int tinyForest2 = 5;

	bool updateDistance = false;
	int currentIDtoCheck = 0;

	[SerializeField]
	bool testing;


	// Update is called once per frame
	void Update()
	{
		if (updateDistance)
		{
			checkForSpecificLocationThroughID(currentIDtoCheck);
		}
	}

	public void ARcanvasActivated(int id)
	{
		currentIDtoCheck = id;
		updateDistance = true;
	}

	public void ARCanvasDeactivated()
	{
		updateDistance = false;
		currentIDtoCheck = 0;
	}

	// auf übereinstimmende location checken, um zugehörigen initializeAR Button zu aktivieren oder Entfernung anzuzeigen
	void checkForSpecificLocationThroughID(int id)
	{
		if (GPSService.Instance.listening || testing) 
		{
			Location currentLocation = new Location("test", 80, 1, 2); //for testing
			if (!testing)
			{
				currentLocation = GPSService.Instance.GetCurrentLocation();
			}
			distanceText = GameObject.FindGameObjectWithTag("DistanceText").GetComponent<TextMeshProUGUI>();

			string locationMatchedClickButton = "Du bist am richtigen Ort. Klicke jetzt den Button";

			if (currentLocation.id == id || testing)
			{
				distanceText.text = locationMatchedClickButton;
				switch (id)
				{
					case 1:
						activateVerticalGarden = GameObject.FindGameObjectWithTag("ButtonInitializeVerticalGarden").GetComponent<Button>();
						activateVerticalGarden.interactable = true;
						break;
					case 2:
						activateUrbanGarden = GameObject.FindGameObjectWithTag("ButtonInitializeUrbanGarden").GetComponent<Button>();
						activateUrbanGarden.interactable = true;
						break;
					case 3:
						activateAtrium = GameObject.FindGameObjectWithTag("ButtonInitializeAtrium").GetComponent<Button>();
						activateAtrium.interactable = true;
						break;
					case 4:
						activateTinyForest1 = GameObject.FindGameObjectWithTag("ButtonInitializeTinyForest1").GetComponent<Button>();
						activateTinyForest1.interactable = true;
						break;
					case 5:
						activateTinyForest2 = GameObject.FindGameObjectWithTag("ButtonInitializeTinyForest2").GetComponent<Button>();
						activateTinyForest2.interactable = true;
						break;
				}
			}
			else
			{
				float distanceToDisplay = Mathf.Round(GPSService.Instance.GetDistanceToPointWithID(id));
				string textToDisplay = "Du bist noch " + distanceToDisplay + " m vom Einstiegspunkt entfernt";
				distanceText.text = textToDisplay;
			}
		}
	}
}
