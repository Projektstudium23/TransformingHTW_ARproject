using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARContentManager : MonoBehaviour
{
    //test stuff

    [SerializeField, Tooltip("testwise -> multiple GameObjectes for the change in years")]
    private GameObject[] testTreeContent;  //-> vielleicht sinnvoller in eigenem Skript1
    private GameObject[] internalTree;
    [SerializeField]
    TextMeshProUGUI changeYearBTNText;
    bool oldYear = false;

    /// <summary>
    /// /////////////////actual used///////////////////////////////
    /// </summary>

    [Tooltip("Drop your AR Content (may be a hole scene) here, s. readme for more details")]
    [SerializeField]
    private GameObject aRContent;
    private GameObject internalARContent;
    bool contentIsSpawned = false;

    private ARPlane[] existingTrackedPlanes;

    private GameObject userCamera;




    private void Start() {
        userCamera = GameObject.Find("Main Camera");
        changeYearBTNText.text = "skip to 2030";
        internalTree = new GameObject[testTreeContent.Length];

        if (userCamera == null) {
            throw new NullReferenceException("userCamera not found -> check string on Gameobject.Find()");
        }
    }



    public void CreateARContent() {
        Transform closestARPlane = null;
        float shortestDistanceToPlane = 100; // doesn`t matter, just something high

        existingTrackedPlanes = GetComponentsInChildren<ARPlane>();

        for (int i = 0; i < existingTrackedPlanes.Length; i++) {
            float distance = DistanceBetweenObjects(existingTrackedPlanes[i].transform.position, userCamera.transform.position);
            if (distance < shortestDistanceToPlane && existingTrackedPlanes[i].alignment == PlaneAlignment.HorizontalUp) { // horizontal planes with normal upwards, floor
                shortestDistanceToPlane = distance;
                closestARPlane = existingTrackedPlanes[i].transform;

            }
        }
        if (closestARPlane == null) {
            new NullReferenceException("No Plane detected-> Plane Tracking failed");
            return;
        }


        if (!contentIsSpawned) {
            Vector3 instantiatePosition = new(userCamera.transform.position.x, closestARPlane.transform.position.y, userCamera.transform.position.z);
            Quaternion instantiateRotation = Quaternion.Euler(new(0, userCamera.transform.rotation.y, 0)); // beim offseten aufpassen,dass das die richtige Richtung ist !
            internalARContent = Instantiate(aRContent, instantiatePosition, instantiateRotation);
            internalARContent.AddComponent<ARPlane>();
            internalARContent.GetComponent<ARPlane>().destroyOnRemoval = false;
            
            /*
            internalTree[0] = Instantiate(testTreeContent[0], instantiatePosition, instantiateRotation);
            internalTree[0].AddComponent<ARAnchor>();
            internalTree[0].GetComponent<ARAnchor>().destroyOnRemoval = false;

            internalTree[1] = Instantiate(testTreeContent[1], instantiatePosition, instantiateRotation);
            internalTree[1].AddComponent<ARAnchor>();
            internalTree[1].GetComponent<ARAnchor>().destroyOnRemoval = false;
            contentIsSpawned = true;

            //test
            oldYear = true;
            changeYear();
            */
        }

    }


    /// <summary>
    /// Distance between a and b without take in account of the height Value (y)
    /// </summary>
    /// <param name="a">ObjectsPosition A</param>
    /// <param name="b">ObjectsPosition B</param>
    /// <returns></returns>
    private float DistanceBetweenObjects(Vector3 a, Vector3 b) {
        float a1 = a.x;
        float a2 = a.z;
        float b1 = b.x;
        float b2 = b.z;
        float x = Mathf.Pow(b1 - a1, 2);
        float y = Mathf.Pow(b2 - a2, 2);
        float distance = Mathf.Sqrt(x + y);
        //   float distance = Vector3.Distance(a.position, b.transform.position);
        return distance;

    }


    public void changeYear() {

        if (!oldYear) {
            oldYear = true;
            internalTree[0].SetActive(false);
            internalTree[1].SetActive(true);
            Debug.Log("old tree initialized");
            internalTree[1].transform.rotation =
               Quaternion.Euler(
                   new(-90f, internalTree[1].transform.rotation.eulerAngles.y, internalTree[1].transform.rotation.eulerAngles.z));
            internalTree[1].transform.localScale *= 2;
            changeYearBTNText.text = "skip back \nto 2020";
        } else {
            oldYear = false;
            //spawn young content
            internalTree[1].SetActive(false);
            internalTree[0].SetActive(true);
            internalTree[0].transform.rotation =
                Quaternion.Euler(
                    new(-90f, internalTree[0].transform.rotation.eulerAngles.y, internalTree[0].transform.rotation.eulerAngles.z));

            changeYearBTNText.text = "skip to 2030";
            Debug.Log("young tree initialized");
        }

    }




}
