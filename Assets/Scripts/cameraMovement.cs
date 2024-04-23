using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float lowerClamp = 2f, upperClamp = 20f;

    private Vector3 _planeForward;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _planeForward = new Vector3(1, 0, 1).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool zoomIn = Input.GetKey(KeyCode.E);
        bool zoomOut = Input.GetKey(KeyCode.Q);
        bool speedDown = Input.GetKeyDown(KeyCode.LeftArrow);
        bool speedUp = Input.GetKeyDown(KeyCode.RightArrow);
        
        //Move on the plane
        Vector3 movementVector = horizontalInput * transform.right + verticalInput * _planeForward;
        transform.position += movementVector * (movementSpeed * Time.deltaTime);
        
        //Zoom
        float zoomInput;
        if (zoomIn)
        {
            zoomInput = -1f;
        }
        else if (zoomOut)
        {
            zoomInput = 1f;
        }
        else
        {
            zoomInput = 0f;
        }
        float newSize = _camera.orthographicSize + zoomInput * Time.deltaTime * movementSpeed;
        _camera.orthographicSize = Mathf.Clamp(newSize, lowerClamp, upperClamp);
        
        //Speed
        if (speedUp)
        {
            Time.timeScale += 0.1f;
        }
        else if (speedDown)
        {
            Time.timeScale -= 0.1f;
        }

    }
}
