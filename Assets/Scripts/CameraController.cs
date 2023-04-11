using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraPivot;
    
    public  float r = 5f, rMin = 2f, rMax = 10f, sens = 2f, speed = 0.5f;
    
    private float _angleX = 0f, _angleY = 0f;

    bool shiftPressed = false;

    public void SetCameraPivot(Transform cameraPivot) {
        this.cameraPivot = cameraPivot;
        transform.LookAt(cameraPivot);
    }

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) shiftPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))   shiftPressed = false;
        
        if ( shiftPressed ) {
            r = Mathf.Clamp(r + (-Input.GetAxis("Vertical")) * (sens/32), rMin, rMax);
        } else {
            r = Mathf.Clamp(r + (-Input.GetAxis("Mouse ScrollWheel")) * sens, rMin, rMax);
            _angleX += -Input.GetAxis("Horizontal") * speed;
            _angleY += Input.GetAxis("Vertical") * speed;
        }

        float x = cameraPivot.position.x + r * Mathf.Sin(_angleX * Mathf.Deg2Rad) * Mathf.Cos(_angleY * Mathf.Deg2Rad);
        float y = cameraPivot.position.y + r * Mathf.Sin(_angleY * Mathf.Deg2Rad);
        float z = cameraPivot.position.z + r * Mathf.Cos(_angleX * Mathf.Deg2Rad) * Mathf.Cos(_angleY * Mathf.Deg2Rad);
        Vector3 newPosition = new Vector3(x, y, z);

        transform.position = newPosition;

        transform.LookAt(cameraPivot);
    }
}
