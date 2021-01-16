using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Source : https://www.youtube.com/watch?v=rnqF6S7PfFA
 * 
 * */

public class BuilderCamera : MonoBehaviour
{
    public Transform followTransform;
    public Transform cameraTransform;
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed=1;
    public float movementTime=1;
    public float rotationAmount=1;
    public Vector3 zoomAmount;
    public Vector3 newPosition;
    public Vector3 newZoom;
    public Quaternion newRotation;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrrentPosition; 
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else {
            HandleMouseInput();
            HandleMovementInput();

        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            followTransform = null;
        }
    }
    void HandleMovementInput() {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else {
            movementSpeed = normalSpeed;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.Q)) {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
        cameraTransform.localPosition  = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
    }

    void HandleMouseInput() {
        if (Input.mouseScrollDelta.y != 0) {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Modud");
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry)) {
                Debug.Log("Plane ray cast");
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {

                Debug.Log("Plane ray cast");
                dragCurrrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition-dragCurrrentPosition;
            }
        }
        if (Input.GetMouseButtonDown(2)) {
            Debug.Log("Midddle Button pressed");
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }
}
