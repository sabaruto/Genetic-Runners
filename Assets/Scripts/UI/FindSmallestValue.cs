using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindSmallestValue : MonoBehaviour {

    private Text text;
    private float currentValue;

    private void Start()
    {
        text = GetComponent<Text>();
        currentValue = 0;
    }

    private void Update()
    {
        if (currentValue != -LineManager.largestValue)
        {
            currentValue = -LineManager.largestValue;
            text.text = "" + Mathf.Floor(currentValue * 100) / 100;
        }
    }
}
