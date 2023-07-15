using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlVisibility : MonoBehaviour
{
    GameObject[] prefabsToShow;

    [SerializeField]
    Slider slider;
    [SerializeField]
    TextMeshProUGUI sliderToText;
    [SerializeField]
    int startYear;

    bool contentIsActive = false;

    private void Start()
    {
        if (slider == null) { Debug.Log("Slider is null!"); }

        slider.onValueChanged.AddListener(delegate { UpdatePrefabVisibility(); });
        slider.onValueChanged.AddListener(delegate { SetSliderToText(); });
    }

    public void ActivateContent()
    {
        slider.value = startYear;
        SetSliderToText();
        Debug.Log("Activated!");
        contentIsActive = true;
        GameObject content = GameObject.FindGameObjectWithTag("AR-Content");
        int l = content.transform.childCount;
        prefabsToShow = new GameObject[l];
        for (int i = 0; i < l; i++)
        {
            prefabsToShow[i] = content.transform.GetChild(i).gameObject;
        }

        UpdatePrefabVisibility(); // Setze die initiale Sichtbarkeit der Prefabs basierend auf dem Slider-Wert
    }

    private void UpdatePrefabVisibility()
    {
        if (!contentIsActive || prefabsToShow == null)
            return;

        int year = Mathf.RoundToInt(slider.value);

        for (int i = 0; i < prefabsToShow.Length; i++)
        {
            GameObject prefab = prefabsToShow[i];

            if (prefab != null)
            {
                if (year >= startYear)
                {
                    prefab.SetActive(true); // Aktiviere das Prefab, wenn der Slider-Wert das Startjahr überschreitet
                }
                else
                {
                    prefab.SetActive(false); // Deaktiviere das Prefab, wenn der Slider-Wert kleiner als das Startjahr ist
                }
            }
        }
    }

    private void SetSliderToText()
    {
        sliderToText.text = slider.value.ToString();
    }
}
