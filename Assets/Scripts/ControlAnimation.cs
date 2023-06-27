using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ControlAnimation : MonoBehaviour
{
  
    Animator animator;
    Slider slider;

    float lengthOfAnimation = 30; //-> must be the length of the animation in frames

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("changeTimeSlider").GetComponent<Slider>();
        if(slider == null) { Debug.Log("slider is null!"); }
      animator = GetComponent<Animator>();
        slider.onValueChanged.AddListener(delegate { changedAnimationState();});
        animator.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
   //    animator.SetFloat("changeState", 20);
    }

    private void changedAnimationState() {

        // animation state name, 0= play time, slide.value =  frame von wo gespielt wird
        float nextFrame = (1f/lengthOfAnimation )* slider.value;
        Debug.Log("slider value: " + slider.value + " normalized: " + nextFrame);
        animator.Play("Linde-Trunk-Growth",0, nextFrame);
        
    }

}
