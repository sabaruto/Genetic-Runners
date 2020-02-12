using UnityEngine;

public class MoveCamera : MonoBehaviour {

    private Camera mainCamera;
    private Vector3 anchorPoint;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        anchorPoint = Vector3.zero;
    }

    // Update is called once per frame
    private void Update ()
    {
        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 3;
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
        {
            anchorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            transform.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - anchorPoint);
        }
	}
}
