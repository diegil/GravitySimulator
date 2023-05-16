using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    public CommonScripts cmS;

    public GameObject closestPlanet;
    private List<Transform> planets;

    public int points = 50;

    public LineRenderer orbitLine;
    public float velocidadOrbita = 100000000f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        orbitLine = GetComponent<LineRenderer>();
        orbitLine.startWidth = 0.05f;
        orbitLine.endWidth = 0.05f;
        orbitLine.material =  new Material(Shader.Find("Sprites/Default"));
        orbitLine.startColor = new Color(255, 255, 255);
        orbitLine.endColor = new Color(255, 255, 255);

        planets = new List<Transform>();
        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        
        foreach (GameObject planetObj in planetsObj)
        {
            planets.Add(planetObj.transform);
        }

        orbitLine.positionCount = points;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");
        if (planetsObj.Length > 2 && GameObject.Find("Pause(Clone)") == null){
            drawOrbit();
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

        orbitLine.SetPositions(orbitPoints);
    }
}