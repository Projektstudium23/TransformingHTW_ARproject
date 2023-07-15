using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARContentCreatorDorian : MonoBehaviour
{
  
    [Tooltip("Drop your AR Content (may be a hole scene) here")]
    [SerializeField]
    private GameObject aRContent;
    private GameObject internalARContent;
    bool contentIsSpawned = false;

    private ARPlane[] existingTrackedPlanes;

    private GameObject userCamera;
    // the users position is the represented by the camera, NOT the XR-Origin



    private void Start() {
        userCamera = GameObject.Find("Main Camera");

        if (userCamera == null) {
            throw new NullReferenceException("userCamera not found -> check string on Gameobject.Find()");
        }
    }



    public void CreateARContent() {
        Transform closestARPlane = null;
        float shortestDistanceToPlane = 100; // doesn`t matter, just something high

        existingTrackedPlanes = GetComponentsInChildren<ARPlane>();
        // find the closest tracked ground plane
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

        // spawn content on specific position and rotation (user depended)
        if (!contentIsSpawned) {
            Vector3 instantiatePosition = new(userCamera.transform.position.x, closestARPlane.transform.position.y, userCamera.transform.position.z); //position of user
            Quaternion instantiateRotation = Quaternion.Euler(new(0, userCamera.transform.rotation.eulerAngles.y, 0)); // correct orientation
            internalARContent = Instantiate(aRContent, instantiatePosition, instantiateRotation);
            internalARContent.AddComponent<ARAnchor>();
            internalARContent.GetComponent<ARAnchor>().destroyOnRemoval = false; 
            
          
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


  
  
    //pretty sure, this does not work properly

   /* public void setupContentForGrowthScaling() {
        GameObject content = GameObject.FindGameObjectWithTag("AR-Content");
        Debug.Log("content: " + content);
        int childrenCount = content.transform.childCount;
        Debug.Log("cildren: " + childrenCount);

        for (int i = 0; i < childrenCount; i++) {

            Transform child = content.transform.GetChild(i);
            GameObject container = Instantiate(new GameObject("scaleContainer-" + i), child.transform);
            container.transform.SetParent(content.transform, true);
            child.transform.SetParent(container.transform, true);
        }
    } */


}
