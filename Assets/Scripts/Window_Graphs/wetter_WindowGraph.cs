

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;
using TMPro;
public class wetter_WindowGraph : MonoBehaviour {

    public int r=0;
    public int g=255;
    public int b=0;
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    public TextAsset csvFile;
    public int separatorCount;
    public bool secondGraph = false; 
    public int maxYValue;

    private bool invertY = false;
    public int strichDicke;
    private RectTransform xLine;
    public int minYValue = 0;
    public int abstand;
    private RectTransform dashTemplateY;

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        List<float> valueList = ReadYValuesFromCSV();
        Debug.Log(valueList[0]);
        List<string> timeList = ReadTimeValuesFromCSV();
        

        if(secondGraph==true){
        ShowGraph(valueList, (float _i) => null, (float _f) => "" + Mathf.RoundToInt(_f+minYValue));
        }
        else{
        ShowGraph(valueList, (float _i) => {
        if(timeList[Mathf.RoundToInt(_i)].EndsWith(":00"))
            return (null) + timeList[Mathf.RoundToInt(_i)];
        else
            return null;
        }, (float _f) => "" + Mathf.RoundToInt(_f));
        }

        }
    

private GameObject CreateCircle(Vector2 anchoredPosition, float value)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image), typeof(EventTrigger));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(33.5f, 33.5f); 
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);


        GameObject tooltip = new GameObject("tooltip", typeof(RectTransform));
        tooltip.transform.SetParent(gameObject.transform, false);


        GameObject imageObj = new GameObject("image", typeof(Image));
        imageObj.transform.SetParent(tooltip.transform, false);
        Image imageComponent = imageObj.GetComponent<Image>(); 
        imageComponent.color = Color.green;
        


        RectTransform imageRectTransform = imageObj.GetComponent<RectTransform>();
        imageRectTransform.anchorMin = new Vector2(0, 0);
        imageRectTransform.anchorMax = new Vector2(1, 1);
        imageRectTransform.pivot = new Vector2(0.5f, 0.5f);
        imageRectTransform.anchoredPosition = Vector2.zero;
        imageRectTransform.sizeDelta = Vector2.zero;


        GameObject textObj = new GameObject("text", typeof(TextMeshProUGUI));
        textObj.transform.SetParent(tooltip.transform, false);
        TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
        textComponent.text = value.ToString();
        textComponent.color = Color.white;
        textComponent.fontSize = 20;
        textComponent.alignment = TextAlignmentOptions.Center;


        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero;
        textRectTransform.sizeDelta = Vector2.zero;


        RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
        tooltipRect.sizeDelta = new Vector2(60, 40);
        tooltipRect.pivot = new Vector2(0, 0.5f);
        tooltipRect.anchoredPosition = new Vector2(50, 0);
        tooltip.SetActive(false);


        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => {
            tooltip.SetActive(!tooltip.activeSelf);
        });
        eventTrigger.triggers.Add(entry);

        return gameObject;
    }





    private void ShowGraph(List<float> valueList, Func<float, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (float _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 0f+maxYValue;
        float xSize = 0f+abstand;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize+i*xSize; 
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), valueList[i]);
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -8f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
/*
            RectTransform xline = Instantiate(xLine);
            xline.SetParent(graphContainer, false);
            xline.gameObject.SetActive(true);
            xline.anchoredPosition = new Vector2(8f,graphHeight);
*/
            
        }
        if(invertY==false) {
            for (int i = 0; i <= separatorCount; i++) {
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.SetParent(graphContainer, false);
                labelY.gameObject.SetActive(true);
                float normalizedValue = i * 1f / separatorCount;
                labelY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
                labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
                RectTransform dashY = Instantiate(dashTemplateY);
                dashY.SetParent(graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(8f, normalizedValue * graphHeight);
        }
        }
        else {
            for (int i = 0; i <= separatorCount; i++) {
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.SetParent(graphContainer, false);
                labelY.gameObject.SetActive(true);
                float normalizedValue = i * 1f / separatorCount;
                labelY.anchoredPosition = new Vector2(1770f, normalizedValue * graphHeight);
                labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
                RectTransform dashY = Instantiate(dashTemplateY);
        }
                
        }
        
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(r,g,b, .60f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 0f+strichDicke);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }


public List<float> ReadYValuesFromCSV()
{
    List<float> yValues = new List<float>();

    // Zeilen der CSV-Datei trennen
    string[] lines = csvFile.text.Split('\n');

    // Durch jede Zeile der CSV-Datei iterieren
    for (int i = 0; i < lines.Length; i++)
    {
        string[] values = lines[i].Split(',');

        if (values.Length >= 2)
        {
            // Y-Wert extrahieren und zur Liste hinzufügen
            float yValue;
            if (float.TryParse(values[1].Replace('.', ','), out yValue))
            {
                yValues.Add(yValue);
            }
            else{}
        }
        else{}
    }

    return yValues;
}


public List<string> ReadTimeValuesFromCSV()
    {
        List<string> timeValues = new List<string>();

        // Zeilen der CSV-Datei trennen
        string[] lines = csvFile.text.Split('\n');

        // Durch jede Zeile der CSV-Datei iterieren
        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length >= 1)
            {
                // Datum und Uhrzeit trennen
                string dateTimeValue = values[0].Trim('"'); // Anführungszeichen entfernen
                string[] dateTimeParts = dateTimeValue.Split(' ');

                if (dateTimeParts.Length >= 2)
                {
                    // Nur die Uhrzeit extrahieren und zur Liste hinzufügen
                    string timeValue = dateTimeParts[1];
                    string[] timeParts = timeValue.Split(':');
                    string formattedTimeValue = $"{timeParts[0]}:{timeParts[1]}"; // Formatieren als "Stunden:Minuten"
                    timeValues.Add(formattedTimeValue);
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }

        return timeValues;
    }
}


 