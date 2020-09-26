using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayController : MonoBehaviour
{
    public GameManager gameManager;
    TextMeshProUGUI text;
    public string valueName;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = gameManager.GetValue(valueName).ToString();   
        gameManager.valueChanged.AddListener(UpdateDisplay);
    }

    void UpdateDisplay(string name, float value)
    {
        if(name == valueName)
        {
            int val = (int) value;
            text.text = val.ToString();
        }
    }
}
