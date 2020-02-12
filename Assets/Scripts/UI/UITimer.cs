using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour {

    private Text text;
    public GeneticAlgorithm algorithm;

    private void Start()
    {
        algorithm = GameObject.FindGameObjectWithTag("Algorithm").GetComponent<GeneticAlgorithm>();
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = "Time: " + algorithm.timer;
    }

}
