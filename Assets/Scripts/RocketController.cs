using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    bool selectingTarget;
    bool seekTarget;

    private GameObject targetPlanet;
    private GameObject hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        seekTarget = false;
        selectingTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectingTarget){
            if(Input.GetMouseButtonDown(0)){
                setHitPoint();
            }
        }

        if(seekTarget){
            this.transform.position = Vector3.MoveTowards(this.transform.position, hitPoint.transform.position, 0.05f);
        }
    }

    void setHitPoint(){
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f)){
            if (hit.collider.tag == "Planet"){
                targetPlanet = hit.collider.gameObject;
                hitPoint = targetPlanet.transform.GetChild(0).gameObject;
                hitPoint.transform.position = hit.point;
                selectingTarget = false;
                seekTarget = true;
                Destroy(GameObject.Find("Pause(Clone)"));
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name != "Earth(Clone)"){
            Destroy(this.gameObject);
        }
    }
}