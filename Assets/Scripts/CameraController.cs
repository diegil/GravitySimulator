using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float moveSpeed;  
    private float rotateSpeed;  
    public float sensitivity;

    private GameObject lookingTarget;
    private bool isLooking;

    public Camera cam;

    bool rotating = false;
    // Start is called before the first frame update
    void Start()
    {
        lookingTarget = this.gameObject;

        sensitivity = 250f;
        moveSpeed = 25f;
        rotateSpeed = 150f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameObject.Find("Pause (Clone)") == null && Input.GetMouseButtonDown(0)){
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)){
                if (hit.collider.tag == "Planet" && hit.collider.gameObject != lookingTarget){
                    lookingTarget = hit.collider.gameObject;
                    isLooking = true;
                }else {
                    resetLookingTarget();
                }
            }
        }

        if (isLooking){
            this.transform.LookAt(lookingTarget.transform);
        }

        //Rotar camara ----------------------------------------------------------------------------------------------
        if (Input.GetMouseButtonDown(2)){
            Cursor.lockState = CursorLockMode.Locked;
            rotating = true;
            resetLookingTarget();
        }
        if (Input.GetMouseButtonUp(2)){
            Cursor.lockState = CursorLockMode.None;
            rotating = false;
        }
        if (rotating){
            cam.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0);
            cam.transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime, 0, 0);
        }

        //Mover y girar camara -------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.W)){
            transform.position += moveSpeed * transform.forward * Time.deltaTime;
            resetLookingTarget();
        }
        if (Input.GetKey(KeyCode.S)){
            transform.position += moveSpeed * -transform.forward * Time.deltaTime;
            resetLookingTarget();
        }
        if (Input.GetKey(KeyCode.A)){
            transform.position += moveSpeed * -transform.right * Time.deltaTime;
            resetLookingTarget();
        }
        if (Input.GetKey(KeyCode.D)){
            transform.position += moveSpeed * transform.right * Time.deltaTime;
            resetLookingTarget();
        }
        if (Input.GetKey(KeyCode.Q)){
            transform.RotateAround(transform.position, transform.forward, rotateSpeed * Time.deltaTime);
            resetLookingTarget();
        }
        if (Input.GetKey(KeyCode.E)){
            transform.RotateAround(transform.position, -transform.forward, rotateSpeed * Time.deltaTime);
            resetLookingTarget();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            moveSpeed = 50f;
            rotateSpeed = 250f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            moveSpeed = 25f;
            rotateSpeed = 150f;
        }

        //Hacer zoom -----------------------------------------------------------------------------------------------
        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            moveSpeed -= Input.GetAxis("Mouse ScrollWheel") / 70;
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 10;
        }
    }

    void resetLookingTarget(){
        isLooking = false;
        lookingTarget = this.gameObject;
    }
}