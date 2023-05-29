using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonScripts : MonoBehaviour
{
    GameObject closestPlanet;
    
    public GameObject calculateClosestBody(){
        float minDistance = float.MaxValue;

        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planetsObj)
        {
            float distance = Vector3.Distance(this.transform.position, planet.transform.position);

            if (distance < minDistance && distance > 0.1 && planet.gameObject.name != "Debris(Clone)")
            {
                minDistance = distance;
                closestPlanet = planet;
            }
        }
        return closestPlanet;
    }
}