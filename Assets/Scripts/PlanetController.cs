using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    private CommonScripts cmS;

    public float mass;

    private float attractionForce;
    private float distance;
    private float g = 6.67384e-11f;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        if(this.name == "Earth(Clone)"){
            mass = 5.972e24f;
        }else if(this.name == "Moon(Clone)"){
            mass = 7.347e22f;
        }

        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        if (planetsObj.Length > 2 && GameObject.Find("Pause(Clone)") == null){
            addCentripetalAcceleration();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] influenceObjects = Physics.OverlapSphere(transform.position, 1e10f);
        
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
            if (obj.name == "Space"){
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

    void addCentripetalAcceleration(){
        GameObject closestPlanet = cmS.calculateClosestBody();

        float w;
        float g = 9.81f;
        float radius = (closestPlanet.transform.position - this.transform.position).magnitude;

        w = Mathf.Sqrt((g / radius));

        Debug.Log(w);

        this.GetComponent<Rigidbody>().AddTorque(Vector3.forward * w * Time.deltaTime, ForceMode.Acceleration);
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
        rigidbody.AddForce( savedVelocity, ForceMode.VelocityChange );
        rigidbody.AddTorque( savedAngularVelocity, ForceMode.VelocityChange );
    }
}
