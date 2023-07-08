
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlUserInputsManager : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI infoText;
	[SerializeField, TextArea]
	string infoTextText;


	[SerializeField]
	Slider slider;
	[SerializeField]
	GameObject spawnBTN;
	[SerializeField]
	Image mask;
	[SerializeField]
	GameObject scanningBar, scanningParent;

	[SerializeField, Tooltip("Count of planes which are generated to proceed to the mask")]
	int detectedPlaneFragments;

	GameObject origin;
	GameObject trackables;
	ControlAnimation controlAnimation;
	ARContentManager contentManager;

	bool tracking = true;


	//testing

	private void Start()
	{
		infoText.text = infoTextText;
		scanningBar.transform.localScale = new(0, 1, 1);
		origin = GameObject.Find("XR Origin");
		controlAnimation = GetComponent<ControlAnimation>();
		contentManager = origin.GetComponent<ARContentManager>();
		slider.gameObject.SetActive(false);
		spawnBTN.SetActive(false);
		mask.gameObject.SetActive(false);
		trackables = GameObject.Find("Trackables");
		Debug.Log("trackables: " + trackables);
		Debug.Log("detected Planes: " + detectedPlaneFragments);

	}

	private void Update()
	{
		if (trackables.transform.childCount > detectedPlaneFragments && tracking)
		{
			tracking = false;
			trackingDone();
		}
		else
		{
			setScaleOfScanningBar(trackables.transform.childCount);
		}
	}

	private void trackingDone()
	{
		scanningParent.SetActive(false);
		mask.gameObject.SetActive(true);
		spawnBTN.gameObject.SetActive(true);
	}

	public void contentSpawned()
	{
		mask.gameObject.SetActive(false);
		scanningBar.transform.parent.gameObject.SetActive(false);
		spawnBTN.gameObject.SetActive(false);
		slider.gameObject.SetActive(true);
		controlAnimation.ActivateContent();
	}


	private void setScaleOfScanningBar(int scale)
	{
		scanningBar.transform.localScale = new((float)scale / 5, 1, 1);
	}

}
