using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour {
    public Text generation;
    private GeneticAlgorithm algorithm;

    private void Start()
    {
        algorithm = GameObject.FindGameObjectWithTag("Algorithm").GetComponent<GeneticAlgorithm>();
    }

    void Update () {
        generation.text = "Generation " + algorithm.generation;
	}
}
