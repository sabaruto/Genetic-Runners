using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

  public GameObject line;
  public static float largestValue = 0;

  private Vector3 startPosition, scaleRatio;
  private GameObject[] lines;

  private void Start()
  {
    startPosition = Camera.main.WorldToScreenPoint(transform.position);
    scaleRatio = transform.localScale / Camera.main.orthographicSize;
    lines = new GameObject[0];
    largestValue = 0;
  }

  public void CreateGraph(List<float> values, Color colour, float width = 1)
  {
    if (values.Count > 1)
    {
      foreach (GameObject line in lines)
      {
        Destroy(line);
      }

      foreach (float value in values) { if (Mathf.Abs(value) > largestValue) { largestValue = Mathf.Abs(value); } }

      lines = new GameObject[values.Count];

      for (int i = 1; i < values.Count; i++)
      {
        lines[i - 1] = Instantiate(line, transform.position, transform.rotation, transform);
        SetLine drawLine = lines[i - 1].GetComponent<SetLine>();

        float x = 6f * Camera.main.orthographicSize / (5 * (values.Count - 1));
        float y = 1.2f * Camera.main.orthographicSize / (5 * largestValue);

        Vector3 start = new Vector3(transform.position.x + x * (i - 1), transform.position.y + y * values[i - 1]);
        Vector3 end = new Vector3(transform.position.x + i * x, transform.position.y + y * values[i]);

        drawLine.DrawLine(start, end, colour, 0.05f);
      }
    }
  }


  private void LateUpdate()
  {
    transform.localScale = scaleRatio * Camera.main.orthographicSize;
    transform.position = Camera.main.ScreenToWorldPoint(startPosition);
  }
}
