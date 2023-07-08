
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlUserInputsManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Component of the infoText")]
    TextMeshProUGUI infoText;
    [SerializeField, TextArea]
    [Tooltip("Text which is displayed on start of teh scene")]
    string infoTextText;


    [SerializeField]

    Slider slider;
    [SerializeField]
    [Tooltip("Button which is accountable for the initialisation of the AR-Content")]
    GameObject spawnBTN;
    [SerializeField]
    [Tooltip("put here the mask of this scene")]
    Image mask;
    [SerializeField]
    [Tooltip("scanning bar and parent for the scanning panels")]
    GameObject scanningBar, scanningParent;

    [SerializeField, Tooltip("Count of planes which are generated to proceed to the mask")]
    int detectedPlaneFragments;

    GameObject trackables; // holds every tracked plane
    ControlAnimation controlAnimation; // logic für


    bool tracking = true; 


    private void Start() {
        infoText.text = infoTextText;
        scanningBar.transform.localScale = new(0, 1, 1);
        controlAnimation = GetComponent<ControlAnimation>();
        slider.gameObject.SetActive(false);
        spawnBTN.SetActive(false);
        mask.gameObject.SetActive(false);
        trackables = GameObject.Find("Trackables");
        Debug.Log("trackables: " + trackables);
        Debug.Log("detected Planes: " + detectedPlaneFragments);
    }

    private void Update() {
        if (trackables.transform.childCount > detectedPlaneFragments && tracking) { // track as long not enough planes found 
            tracking = false;
            trackingDone();
        } else {
            setScaleOfScanningBar(trackables.transform.childCount);
        }
    }
    
    private void trackingDone() {
        scanningParent.SetActive(false);
        mask.gameObject.SetActive(true);
        spawnBTN.gameObject.SetActive(true);
    }
    /// <summary>
    /// If content is spawned (onClick initiateBTN) update canvas
    /// </summary>
    public void contentSpawned() {
        mask.gameObject.SetActive(false);
        scanningBar.transform.parent.gameObject.SetActive(false);
        spawnBTN.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
        controlAnimation.ActivateContent();
    }


    private void setScaleOfScanningBar(int scale) {
        scanningBar.transform.localScale = new((float)scale / 5, 1, 1);
    }

}
