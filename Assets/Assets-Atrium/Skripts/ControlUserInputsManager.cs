
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
    TextMeshProUGUI scanningState;
    [SerializeField]
    string scanningMessage;
    [SerializeField]
    string scanningMessageDone;
    [SerializeField, Tooltip("Count of planes which are generated to proceed to the mask")]
    int detectedPlaneFragments;

    GameObject origin;
    GameObject trackables;
    ControlAnimation controlAnimation;
    ARContentManager contentManager;

    bool tracking = true;

    private void Start() {
        infoText.text = infoTextText;

        origin = GameObject.Find("XR Origin");
        controlAnimation = GetComponent<ControlAnimation>();
        contentManager = origin.GetComponent<ARContentManager>();
        slider.gameObject.SetActive(false);
        spawnBTN.SetActive(false);
        mask.gameObject.SetActive(false);
        scanningState.text = scanningMessage;
        trackables = GameObject.Find("Trackables");
        Debug.Log("trackables: " + trackables);
        Debug.Log("detected Planes: " + detectedPlaneFragments);

    }

    private void Update() {
        if (trackables.transform.childCount > detectedPlaneFragments && tracking) {
            tracking = false;
            trackingDone();
        }
    }

    private void trackingDone() {
        scanningState.text = scanningMessageDone;
        mask.gameObject.SetActive(true);
        spawnBTN.gameObject.SetActive(true);
    }

    public void contentSpawned() {
        mask.gameObject.SetActive(false);
        scanningState.transform.parent.gameObject.SetActive(false);
        spawnBTN.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
        controlAnimation.ActivateContent();
    }


}
