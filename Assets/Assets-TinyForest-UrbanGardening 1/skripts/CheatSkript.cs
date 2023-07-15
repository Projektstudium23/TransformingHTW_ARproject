using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabsToShow;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private int startYear = 2023;

    private int visiblePrefabCount = 0;

    private void Start()
    {
        SetPrefabsVisibility(0); // Mache zu Beginn keine Prefabs sichtbar

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        slider.value = startYear;
    }

    private void OnSliderValueChanged(float value)
    {
        int year = Mathf.RoundToInt(value);

        if (year > startYear)
        {
            int prefabCount = Mathf.Min(year - startYear, prefabsToShow.Length); // Berechne die Anzahl der anzuzeigenden Prefabs basierend auf dem Slider-Wert
            SetPrefabsVisibility(prefabCount); // Mache entsprechende Anzahl von Prefabs sichtbar
        }
        else
        {
            SetPrefabsVisibility(0); // Mache keine Prefabs sichtbar, wenn der Slider-Wert den Startwert erreicht oder darunter liegt
        }
    }

    private void SetPrefabsVisibility(int count)
    {
        for (int i = 0; i < prefabsToShow.Length; i++)
        {
            if (i < count)
            {
                prefabsToShow[i].SetActive(true);
            }
            else
            {
                prefabsToShow[i].SetActive(false);
            }
        }

        visiblePrefabCount = count;
    }
}