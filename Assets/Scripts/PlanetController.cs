﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    private CommonScripts cmS;

    public float mass;

    //gravity force requirements
    private float attractionForce;
    private float distance;
    private float g = 6.67384e-11f;

    //pause game requirements
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        cmS = gameObject.GetComponent<CommonScripts>();

        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        if (this.gameObject.name == "Moon(Clone)"){
            addCentripetalAcceleration();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mass > 1e16f || this.gameObject.name == "Rocket(Clone)"){
            Collider[] influenceObjects = Physics.OverlapSphere(transform.position, float.MaxValue);

            if (GameObject.Find("Pause(Clone)") != null && pause == false){
                pause = true;
                PauseGame();
            }else if (GameObject.Find("Pause(Clone)") == null && pause == true){
                ResumeGame();
                pause = false;
            }

            foreach (var obj in influenceObjects){

                if (influenceObjects.Length == 2){
                    continue;
                }
                if (obj.tag != "Planet"){
                    continue;
                }
                if (obj.name == "Earth(Clone)"){
                    continue;
                }

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                Vector3 forceDirection = (transform.position - rb.position);

                distance = forceDirection.sqrMagnitude;

                if (distance < 0.1){
                    continue;
                }

                attractionForce = ((mass * rb.mass) / (distance * 10e12f)) * g;

                rb.AddForce(forceDirection * attractionForce);
            }
        }
    }

    void addCentripetalAcceleration(){
        GameObject closestPlanet = cmS.calculateClosestBody();

        float w;
        Vector3 radiusVector = closestPlanet.transform.position - this.transform.position;
        float radius = radiusVector.magnitude * 4e11f;
        Vector3 yVector = new Vector3(this.transform.position.x, 1, this.transform.position.z) - this.transform.position;
        radiusVector = radiusVector.normalized;
        yVector = yVector.normalized;
        Vector3 perpendicularRadiusVector3D = Vector3.Cross(radiusVector, yVector).normalized;

        w = Mathf.Sqrt((g * closestPlanet.GetComponent<PlanetController>().mass) / radius);

        // Debug.Log(w);
        this.GetComponent<Rigidbody>().AddForce(perpendicularRadiusVector3D * w, ForceMode.VelocityChange);
    }

    void PauseGame(){
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        savedVelocity = rigidbody.velocity;
        savedAngularVelocity = rigidbody.angularVelocity;
        rigidbody.isKinematic = true;
    }

    void ResumeGame(){
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(savedVelocity, ForceMode.VelocityChange);
        rigidbody.AddTorque(savedAngularVelocity, ForceMode.VelocityChange);
    }
}