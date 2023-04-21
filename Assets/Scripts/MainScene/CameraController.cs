using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    public Transform cameraPivot;
    
    public  float r = 5f, rMin = 2f, rMax = 10f, sens = 2f, speed = 0.5f;

    private float _angleX  = 0f,    _angleY = 0f;
    private float _dumping = 0.03f, _treshhold = 0.5f;

    private bool    shiftPressed           = false;
    private bool    leftMouseButtonPressed = false;
    private Vector3 velocity               = Vector3.zero;
   

    private Vector3 lastMousePosition;

    public void SetCameraPivot(Transform cameraPivot) {
        this.cameraPivot = cameraPivot;
        transform.LookAt(cameraPivot);
    }

    void Update() {
        MoveOnStart         ();
        IsShiftPressed      ();
        ZoomControl         ();
        KeyboardAngleControl();
        MouseAngleControl   ();
        ComputePosition     ();
    }

    private void MoveOnStart() {
        if (Input.GetMouseButtonDown(0)) leftMouseButtonPressed = true;
        if (!leftMouseButtonPressed) _angleX += speed * 0.1f;
        
    }

    public void Zoom(float axis) {
        r = Mathf.Clamp(r + axis * sens, rMin, rMax);
    }

    private void ZoomControl() {
        r = Mathf.Clamp(r + 
            (-Input.GetAxis(shiftPressed ? "Vertical" : "Mouse ScrollWheel")) * 
                (sens / (shiftPressed ? 32 : 1)), rMin, rMax);
    }

    private void KeyboardAngleControl() {
        if (!shiftPressed) {
            _angleX += -Input.GetAxis("Horizontal") * speed;
            _angleY += Input.GetAxis("Vertical") * speed;
        }
    }

    private void MouseAngleControl() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                lastMousePosition = Input.mousePosition;
                velocity = Vector3.zero;
            } else if (Input.GetMouseButton(0)) {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
                _angleX += mouseDelta.x * sens * 2 * Time.deltaTime;
                _angleY += -mouseDelta.y * sens * 2 * Time.deltaTime;
                lastMousePosition = Input.mousePosition;
                velocity = mouseDelta * _dumping;
            } else if (velocity.magnitude > _treshhold) {
                _angleX += velocity.x * sens * 4 * Time.deltaTime;
                _angleY += -velocity.y * sens * 4 * Time.deltaTime;
                velocity *= (1f - _dumping);
            }
        }
    }

    private void ComputePosition() {
        transform.position = new Vector3(
            cameraPivot.position.x + r * Mathf.Sin(_angleX * Mathf.Deg2Rad) * Mathf.Cos(_angleY * Mathf.Deg2Rad),
            cameraPivot.position.y + r * Mathf.Sin(_angleY * Mathf.Deg2Rad),
            cameraPivot.position.z + r * Mathf.Cos(_angleX * Mathf.Deg2Rad) * Mathf.Cos(_angleY * Mathf.Deg2Rad)
        );
        transform.LookAt(cameraPivot);
    }

    private void IsShiftPressed() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) shiftPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) shiftPressed = false;
    }
}
