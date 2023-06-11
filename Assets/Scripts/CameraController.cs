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

    private GameObject followingTarget;
    private bool isFollowing;

    public Camera cam;

    bool rotating = false;
    // Start is called before the first frame update
    void Start()
    {
        lookingTarget = this.gameObject;
        followingTarget = this.gameObject;

        sensitivity = 250f;
        moveSpeed = 25f;
        rotateSpeed = 150f;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //Apuntar la camara a un objeto------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0)){
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.MaxValue)){
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

        //Hacer que la camara siga a un objeto-----------------------------------------------------------------------
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1)){
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.MaxValue)){
                if (hit.collider.tag == "Planet" && hit.collider.gameObject != followingTarget){
                    followingTarget = hit.collider.gameObject;
                    isFollowing = true;
                }else {
                    resetFollowingTarget();
                }
            }
        }

        if (isFollowing){
            this.transform.SetParent(followingTarget.transform);
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
            resetFollowingTarget();
        }
        if (Input.GetKey(KeyCode.S)){
            transform.position += moveSpeed * -transform.forward * Time.deltaTime;
            resetLookingTarget();
            resetFollowingTarget();
        }
        if (Input.GetKey(KeyCode.A)){
            transform.position += moveSpeed * -transform.right * Time.deltaTime;
            resetLookingTarget();
            resetFollowingTarget();
        }
        if (Input.GetKey(KeyCode.D)){
            transform.position += moveSpeed * transform.right * Time.deltaTime;
            resetLookingTarget();
            resetFollowingTarget();
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
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && isLooking){
            moveSpeed -= Input.GetAxis("Mouse ScrollWheel") / 70;
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 10;
        }
    }

    void resetLookingTarget(){
        isLooking = false;
        lookingTarget = this.gameObject;
        cam.fieldOfView = 60;
    }

    void resetFollowingTarget(){
        isFollowing = false;
        followingTarget = this.gameObject;
        this.gameObject.transform.SetParent(null);
    }
}