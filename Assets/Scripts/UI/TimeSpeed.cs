using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeed : MonoBehaviour {

    private float speed;

    private void Update()
    {
        speed = GetComponent<Slider>().value;
        Time.timeScale = speed;
        Time.fixedDeltaTime = 0.02f;
    }
}
