using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private bool selectingTarget;
    private bool liftoff;
    private bool seekTarget;

    private GameObject targetPlanet;
    private GameObject hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        selectingTarget = true;
        liftoff = false;
        seekTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        try {
            if(this.transform.position == GameObject.Find("Launchpad2").transform.position){
                // Destroy(GameObject.Find("Launchpad2"));
                liftoff = false;
                seekTarget = true;
            }
        }catch(Exception){}

        if(selectingTarget){
            if(Input.GetMouseButtonDown(0)){
                setHitPoint();
            }
        }

        if(liftoff){
            this.transform.position = Vector3.MoveTowards(this.transform.position, GameObject.Find("Launchpad2").transform.position, 0.05f);
        }

        if(seekTarget){
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPlanet.transform.position, 0.05f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Planet"){
            if (other.gameObject.name == targetPlanet.gameObject.name){

                float impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
                float impactEnergy = 0.5f * this.gameObject.GetComponent<PlanetController>().mass * Mathf.Pow(impactVelocity, 2);

                Vector3 explosionCenter = this.gameObject.transform.position;

                float explosionRadius = 130 * Mathf.Pow(this.gameObject.GetComponent<PlanetController>().mass, (1/6));

                targetPlanet.gameObject.GetComponent<Rigidbody>().AddExplosionForce(impactEnergy, explosionCenter, explosionRadius);
                
                Destroy(this.gameObject);
            }
        }
    }

    void setHitPoint(){
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f)){
            if (hit.collider.tag == "Planet"){
                targetPlanet = hit.collider.gameObject;
                selectingTarget = false;
                liftoff = true;
                Destroy(GameObject.Find("Pause(Clone)"));
            }
        }
    }
}