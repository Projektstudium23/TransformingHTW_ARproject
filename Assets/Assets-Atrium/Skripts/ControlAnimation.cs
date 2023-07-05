using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControlAnimation : MonoBehaviour
{

    Animator animator;

    [SerializeField]
    Slider slider;
    [SerializeField]
    TextMeshProUGUI sliderToText;
    [SerializeField]
    int startYear;

    GameObject[] scalableObjects;

    bool contentIsActive = false;

    float lengthOfAnimation = 30; //-> must be the length of the animation in frames

    // Start is called before the first frame update
    void Start() {

        if (slider == null) { Debug.Log("slider is null!"); }
        //   animator = GetComponent<Animator>();
        //    slider.onValueChanged.AddListener(delegate { changedAnimationState(); });
        slider.onValueChanged.AddListener(delegate { changeScaleObjects(); });
        slider.onValueChanged.AddListener(delegate { SetSliderToText(); });
        
        //  animator.speed = 0f;
    }


    private void changedAnimationState() {

        // animation state name, 0= play time, slide.value =  frame von wo gespielt wird
        float nextFrame = (1f / lengthOfAnimation) * slider.value;
        Debug.Log("slider value: " + slider.value + " normalized: " + nextFrame);
        animator.Play("Linde-Trunk-Growth", 0, nextFrame);
    }

    private void changeScaleObjects() {
        if (contentIsActive) {
           
            float val  = (slider.value - slider.minValue) / (slider.maxValue - slider.minValue); // lineares Skalieren

            for (int i = 0; i < scalableObjects.Length; i++) {
                scalableObjects[i].transform.localScale = new(val, val, val);
            }
        }
    }

    public void ActivateContent() {

        slider.value = startYear;
        SetSliderToText();
        Debug.Log("activated!");
        contentIsActive = true;
        GameObject content = GameObject.FindGameObjectWithTag("AR-Content");
        int l = content.transform.childCount;
        scalableObjects = new GameObject[l];
        for (int i = 0; i < l; i++) {
            scalableObjects[i] = content.transform.GetChild(i).gameObject;
        }
        changeScaleObjects();
    }

    private void SetSliderToText() {
        sliderToText.text = slider.value.ToString();
    }
}
