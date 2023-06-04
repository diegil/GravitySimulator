using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketController : MonoBehaviour
{
    private bool selectingTarget;
    private bool liftoff;
    private bool seekTarget;

    private GameObject targetPlanet;
    private GameObject hitPoint;

    public float explosionMegatons;
    private float explosionForceMass;

    public GameObject debrisPrefab;

    private LineRenderer targetLine;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        targetLine = gameObject.GetComponent<LineRenderer>();
        targetLine.enabled = false;
        targetLine.startWidth = 0.2f;
        targetLine.endWidth = 0.2f;
        targetLine.material =  new Material(Shader.Find("Sprites/Default"));
        targetLine.material.SetColor("_Color", Color.red);
        targetLine.positionCount = 2;

        explosionForceMass = explosionMegatons * 1e9f;
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
                targetLine.enabled = true;
            }
        }catch(Exception){}

        if(selectingTarget){
            if(Input.GetMouseButtonDown(0)){
                selectTarget();
            }
        }

        if(liftoff){
            this.transform.position = Vector3.MoveTowards(this.transform.position, GameObject.Find("Launchpad2").transform.position, 0.05f);
        }

        if(seekTarget){
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPlanet.transform.position, 0.05f);
            Vector3[] points = new Vector3[2];
            points[0] = this.transform.position;
            points[1] = targetPlanet.transform.position;
            targetLine.SetPositions(points);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Planet"){
            if (other.gameObject.name == targetPlanet.gameObject.name){

                float impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                //5.8776e15f este numero es la relacion entre la masa del script "planet controller" de la luna y la masa de su rigidbody
                //la fuerza que tiene un megaton es de 4,184e+15N
                //La fuerza de la explosion de calcula multiplicando los megatones por los newtons de fuerza de 1 megaton (luego lo divido para compensar las masas)
                float explosionForce = (explosionMegatons * 4.184e15f) / 5.877e15f;

                Vector3 explosionCenter = this.gameObject.transform.position;
                
                float expo = 1f/6f;
                float explosionRadius = 130 * Mathf.Pow(explosionForceMass, expo);

                //TEORIA: Sabiendo el radio de la explosion se puede supuestamente saber la profundidad de la misma, con estos dos datos se podria calcular el volumen de masa 
                //movido por la explosion -> depth = diameter * 0.2/0.3
                float explosionDepth = explosionRadius * Random.Range(0.2f, 0.3f);

                float displacedVolume = Mathf.PI * Mathf.Pow(explosionRadius, 2) * explosionDepth;

                float moonDensity = 3340; //kg/m3

                float displacedWeight = displacedVolume * moonDensity;
                
                createDebris(displacedWeight);

                targetPlanet.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionCenter, explosionRadius);
                
                Destroy(this.gameObject);
            }
        }
    }

    void selectTarget(){
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

    void createDebris(float displacedWeight){
        //Numero de fragmentos que saldran tras la explosion
        int debrisNumber = Random.Range(50, 100);

        //Numero de fragmentos en funcion de la masa (massive, medium, small)
        int massiveDebris = Random.Range(3, debrisNumber / 8);
        int debrisLeft = debrisNumber - massiveDebris;
        
        int mediumDebris = Random.Range(5,  debrisLeft / 4);
        debrisLeft -= mediumDebris;

        Debug.Log(debrisNumber);
        float[] debrisMass = new float[debrisNumber]; 
        Debug.Log(debrisMass.Length);
        calculateDebrisMass(displacedWeight, debrisNumber, massiveDebris, debrisLeft, mediumDebris, debrisMass);
        

        for (int i = 0; i <= debrisMass.Length - 1; i++){
            GameObject newDebris;
            newDebris = Instantiate(debrisPrefab, this.transform.position, Quaternion.identity);
            targetPlanet.GetComponent<PlanetController>().mass -= displacedWeight;
            newDebris.GetComponent<PlanetController>().mass = debrisMass[i];
            if (i < massiveDebris){
                newDebris.GetComponent<Transform>().localScale *= debrisMass[i] / 1e18f;
                newDebris.GetComponent<TrailRenderer>().startWidth *= debrisMass[i] / 1e18f;
                newDebris.GetComponent<TrailRenderer>().endWidth *= debrisMass[i] / 1e18f;
            }else if (i >= massiveDebris && i < mediumDebris + massiveDebris){
                newDebris.GetComponent<Transform>().localScale *= debrisMass[i] / 1e17f;
                newDebris.GetComponent<TrailRenderer>().startWidth *= debrisMass[i] / 1e17f;
                newDebris.GetComponent<TrailRenderer>().endWidth *= debrisMass[i] / 1e17f;
            }else if (i >= massiveDebris + mediumDebris){
                newDebris.GetComponent<Transform>().localScale *= debrisMass[i] / 1e16f;
                newDebris.GetComponent<TrailRenderer>().startWidth *= debrisMass[i] / 1e16f;
                newDebris.GetComponent<TrailRenderer>().endWidth *= debrisMass[i] / 1e16f;
            }
        }

        // GameObject newDebris = new GameObject();
        
    }

    void calculateDebrisMass(float displacedWeight, int debrisNumber, int massiveDebris, int debrisLeft, int mediumDebris, float[] debrisMass){
        

        //Masa asignada a los fragmentos "massive"
        float massiveDebrisTotalMass = displacedWeight * Random.Range(0.9f, 0.95f);
        displacedWeight -= massiveDebrisTotalMass;

        //Masa asignada a los fragmentos "medium"
        float mediumDebrisTotalMass = displacedWeight * Random.Range(0.99f, 0.999f);
        displacedWeight -= mediumDebrisTotalMass;

        //Masa de los fragmentos "massive" dividida entre ellos
        float massiveDebrisSingleMass = massiveDebrisTotalMass / massiveDebris;
        float massiveDebrisLeftMass = 0;

        //Masa de los fragmentos "medium" dividida entre ellos
        float mediumDebrisSingleMass = mediumDebrisTotalMass / mediumDebris;
        float mediumDebrisLeftMass = 0;

        //Masa de los fragmentos "small" dividida entre ellos
        float smallDebrisSingleMass = displacedWeight / debrisLeft;
        float smallDebrisLeftMass = 0;

        for (int i = 0; i <= debrisNumber - 1; i++){
            Debug.Log(i);
            if (i < massiveDebris){
                
                //Masa que se le quita a cada fragmento "massive"
                float massiveDebrisDifferenceMass = Random.Range(0, massiveDebrisSingleMass / 4);

                //Masa que queda "suelta" de todos los fragmentos "massive"
                massiveDebrisLeftMass += massiveDebrisDifferenceMass;

                //Masa asignada a cada fragmento "massive"
                debrisMass[i] = massiveDebrisSingleMass - massiveDebrisDifferenceMass;

            }else if (i >= massiveDebris && i < mediumDebris + massiveDebris){

                //Masa que se le quita a cada fragmento "medium"
                float mediumDebrisDifferenceMass = Random.Range(0, mediumDebrisSingleMass / 4);

                //Masa que queda "suelta" de todos los fragmentos "medium"
                mediumDebrisLeftMass += mediumDebrisDifferenceMass;

                //Masa asignada a cada fragmento "medium"
                debrisMass[i] = mediumDebrisSingleMass - mediumDebrisDifferenceMass;
                 
            }else if (i >= massiveDebris + mediumDebris){

                //Masa que se le quita a cada fragmento "small"
                float smallDebrisDifferenceMass = Random.Range(0, smallDebrisSingleMass / 4);

                //Masa que queda "suelta" de todos los fragmentos "small"
                smallDebrisLeftMass += smallDebrisDifferenceMass;

                //Masa asignada a cada fragmento "small"
                debrisMass[i] = smallDebrisSingleMass - smallDebrisDifferenceMass;
            }
        }

        //Masa "suelta" de los fragmentos "massive" dividida entre todos ellos
        float massiveDebrisLeftSingleMass = massiveDebrisLeftMass / massiveDebris;

        //Masa "suelta" de los fragmentos "medium" dividida entre todos ellos
        float mediumDebrisLeftSingleMass = mediumDebrisLeftMass / mediumDebris;

        //Masa "suelta" de los fragmentos "small" dividida entre todos ellos
        float smallDebrisLeftSingleMass = smallDebrisLeftMass / debrisLeft;

        for (int i = 0; i <= debrisMass.Length - 1; i++){
            if (i < massiveDebris){

                //Masa final de cada fragmento "massive" (masa asignada anteriormente + masa suelta que le corresponde a cada uno)
                debrisMass[i] += massiveDebrisLeftSingleMass;

            }else if (i >= massiveDebris && i < mediumDebris + massiveDebris){

                //Masa final de cada fragmento "medium" (masa asignada anteriormente + masa suelta que le corresponde a cada uno)
                debrisMass[i] += mediumDebrisLeftSingleMass;

            }else if (i > massiveDebris + mediumDebris){

                //Masa final de cada fragmento "small" (masa asignada anteriormente + masa suelta que le corresponde a cada uno)
                debrisMass[i] += smallDebrisLeftSingleMass;
            }
        }
    }
}