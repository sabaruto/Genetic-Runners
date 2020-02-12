using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMarkers : MonoBehaviour {

    public GameObject Marker;

    private void Start()
    {
        for(int i = -100; i <= 100; i+= 10)
        {
            GameObject distanceMarker = Instantiate(Marker, new Vector2(i, -1), transform.rotation, transform);
            distanceMarker.GetComponent<Text>().text = "" + i;
        }
    }

}
