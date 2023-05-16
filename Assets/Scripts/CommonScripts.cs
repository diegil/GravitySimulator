using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonScripts : MonoBehaviour
{
    public GameObject calculateClosestBody(){
        GameObject closestPlanet = new GameObject();
        float minDistance = float.MaxValue;

        GameObject[] planetsObj = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planetsObj)
        {
            float distance = Vector3.Distance(this.transform.position, planet.transform.position);

            if (distance < minDistance && distance > 0.1)
            {
                minDistance = distance;
                closestPlanet = planet;
            }
        }
        return closestPlanet;
    }
}
