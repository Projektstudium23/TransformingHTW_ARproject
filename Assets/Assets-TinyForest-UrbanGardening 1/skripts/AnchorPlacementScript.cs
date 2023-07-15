using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorPlacementScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    private ARAnchorManager anchorManager;

    private void Start()
    {
        anchorManager = FindObjectOfType<ARAnchorManager>();
    }

    public void PlaceAnchorsOnClick()
    {
        if (anchorManager != null)
        {
            foreach (var prefab in prefabs)
            {
                ARAnchor anchor = anchorManager.AddAnchor(new Pose(Camera.main.transform.position, Camera.main.transform.rotation));
                prefab.transform.SetParent(anchor.transform);
            }
        }
    }
}