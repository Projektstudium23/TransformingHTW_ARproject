using UnityEngine;
using UnityEngine.UI;

public class PrefabsSliderController : MonoBehaviour
{
    public GameObject[] prefabs;  // Array mit den zu steuernden Prefabs
    public Slider slider;         // Referenz zum Slider im Interface

    private void Start()
    {
        slider.onValueChanged.AddListener(UpdatePrefabVisibility);
    }

    private void UpdatePrefabVisibility(float value)
    {
        int numVisiblePrefabs = Mathf.RoundToInt(value);

        // Aktualisiere die Sichtbarkeit der Prefabs basierend auf dem Slider-Wert
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (i < numVisiblePrefabs)
            {
                prefabs[i].SetActive(true);
            }
            else
            {
                prefabs[i].SetActive(false);
            }
        }
    }
}
