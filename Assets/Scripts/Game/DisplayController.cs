using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayController : MonoBehaviour
{
    public GameManager gameManager;
    TextMeshProUGUI text;
    public string valueName;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = gameManager.GetValue(valueName).ToString();   
        gameManager.valueChanged.AddListener(UpdateDisplay);
    }

    // Update is called once per frame
    void UpdateDisplay(string name, float value)
    {
        if(name == valueName)
        {
            int val = (int) value;
            text.text = val.ToString();
        }
    }
}
