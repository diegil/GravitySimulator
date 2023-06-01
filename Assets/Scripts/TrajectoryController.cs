using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    private CommonScripts cmS;

    private int points = 50;

    private LineRenderer orbitLine;

    // Start is called before the first frame update
    void Start()
    {
        cmS = gameObject.GetComponent<CommonScripts>();
        
        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        if(this.gameObject.name != "Rocket(Clone)"){
            gameObject.AddComponent<LineRenderer>();
            orbitLine = GetComponent<LineRenderer>();
            orbitLine.startWidth = 0.05f;
            orbitLine.endWidth = 0.05f;
            orbitLine.material =  new Material(Shader.Find("Sprites/Default"));
            orbitLine.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        if (planetsObj.Length > 1){
            // drawOrbit();
            if(this.gameObject.name != "Rocket(Clone)"){
                drawLine();
            }
        }
    }

    void drawLine(){
        GameObject closestPlanet = cmS.calculateClosestBody();

        float distanceToClosestPlanet = (closestPlanet.transform.position - this.transform.position).magnitude;
        if(distanceToClosestPlanet <= 10){
            orbitLine.enabled = true;
            orbitLine.material.SetColor("_Color", Color.red);
        }else if(distanceToClosestPlanet > 10 && distanceToClosestPlanet <= 150){
            orbitLine.enabled = true;
            orbitLine.material.SetColor("_Color", Color.green);
        }else if(distanceToClosestPlanet > 150 && distanceToClosestPlanet <= 200){
            orbitLine.enabled = true;
            orbitLine.material.SetColor("_Color", Color.blue);
        }else if(distanceToClosestPlanet > 200 && distanceToClosestPlanet <= 300){
            orbitLine.enabled = false;
        }else if(distanceToClosestPlanet > 300 && this.gameObject.name != "Moon(Clone)" && this.gameObject.name != "Earth(Clone)"){
            Destroy(this.gameObject);
        }

        Vector3[] linePoints = new Vector3[2];
        linePoints[0] = closestPlanet.transform.position;
        linePoints[1] = this.transform.position;
        if (this.GetComponent<PlanetController>().mass < closestPlanet.GetComponent<PlanetController>().mass){
            orbitLine.positionCount = 2;
            orbitLine.SetPositions(linePoints);
        }
    }

    void drawOrbit(){
        GameObject closestPlanet = cmS.calculateClosestBody();

        float radius = (closestPlanet.transform.position - this.transform.position).magnitude;
        
        Vector3[] orbitPoints = new Vector3[points];

        for (int i = 0; i < points; i++)
        {
            float angle = (i * 1.0f / points) * Mathf.PI * 2f;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            orbitPoints[i] = closestPlanet.transform.position + position;
        }

        if (this.GetComponent<PlanetController>().mass < closestPlanet.GetComponent<PlanetController>().mass){
            orbitLine.enabled = true;
            orbitLine.positionCount = points;
            orbitLine.SetPositions(orbitPoints);
        }
    }
}