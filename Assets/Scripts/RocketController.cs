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
        if (Physics.Raycast(ray, out hit, float.MaxValue)){
            if (hit.collider.tag == "Planet"){
                targetPlanet = hit.collider.gameObject;
                selectingTarget = false;
                liftoff = true;
                Destroy(GameObject.Find("Pause(Clone)"));
            }
        }
    }

    void createDebris(float displacedWeight){
        List<float> fragmentsMass = calculateDebrisMass(displacedWeight);
        for (int i = 0; i <= fragmentsMass.Count - 1; i++){
            GameObject newDebris;
            newDebris = Instantiate(debrisPrefab, this.transform.position, Quaternion.identity);
            newDebris.GetComponent<PlanetController>().mass = fragmentsMass[i];
        }
    }

    List<float> calculateDebrisMass(float displacedWeight){
        List<float> fragmentsMass = new List<float>();
        while(displacedWeight > 1){
            float fragmentMass = Random.Range(1, displacedWeight / 3);
            displacedWeight -= fragmentMass;
            fragmentsMass.Add(fragmentMass);
        }
            Debug.Log(fragmentsMass.Count);
        return fragmentsMass;
    }
}