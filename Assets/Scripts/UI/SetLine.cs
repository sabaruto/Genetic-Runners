using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLine : MonoBehaviour {

    public GameObject startObject, endObject, midObject;

    public void DrawLine(Vector3 start, Vector3 end, Color colour, float width = 1)
    {
        Vector2 direction = end - start;
        midObject.transform.position = (start + end) / 2;
        midObject.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        midObject.transform.localScale = new Vector3(direction.magnitude * 5 / Camera.main.orthographicSize, width, 1);

        startObject.transform.position = start;
        endObject.transform.position = end;

        startObject.transform.localScale = endObject.transform.localScale = new Vector3(width, width, 1);
        midObject.GetComponent<SpriteRenderer>().color = startObject.GetComponent<SpriteRenderer>().color = endObject.GetComponent<SpriteRenderer>().color = colour;
    }
}
