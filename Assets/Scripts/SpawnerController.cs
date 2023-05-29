using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private GameObject previewObj;

    private GameObject pauseObj;
    public GameObject pausePrefab;
    
    public GameObject rocketPrefab;

    public GameObject[] planets;
    int selectedPlanet = -1;

    bool followPreview = false;

    // Update is called once per frame
    void Update()
    {
        //Comprueba que tecla se pulsa para crear un planeta u otro
        if(Input.GetKeyDown(KeyCode.A) && GameObject.Find("Pause(Clone)") == null && GameObject.Find("Earth(Clone)") == null){
            selectedPlanet = 0;
            pauseGame();
            CreatePreview(selectedPlanet);
        }else if(Input.GetKeyDown(KeyCode.B) && GameObject.Find("Pause(Clone)") == null && GameObject.Find("Moon(Clone)") == null){
            selectedPlanet = 1;
            pauseGame();
            CreatePreview(selectedPlanet);
        }else if(Input.GetKeyDown(KeyCode.C) && GameObject.Find("Pause(Clone)") == null && GameObject.Find("Earth(Clone)") != null){
            pauseGame();
            GameObject launchpad = GameObject.Find("Launchpad");
            Instantiate(rocketPrefab, launchpad.transform.position, Quaternion.identity);
        }

        //Hace que la preview del planeta siga al cursor
        if(followPreview){
            previewObj.GetComponent<TrailRenderer>().enabled = false;
            
            Ray followRay;
            RaycastHit followHit;
            followRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(followRay, out followHit, 100.0f)){
                if (followHit.collider.name == "Space"){
                    previewObj.transform.position = followHit.point;
                }
            }
        }

        //Comprueba si se presiona el click izquierdo para crear el planeta o si se presiona click derecho para no crearlo
        if (Input.GetMouseButtonDown(0) && followPreview == true){
            resumeGame();
            Instantiate(planets[selectedPlanet], previewObj.transform.position, Quaternion.identity);
        }else if(Input.GetMouseButtonDown(1) && followPreview == true){
            resumeGame();
        }
    }

    void CreatePreview(int selectedPlanet){
        Ray previewRay;
        RaycastHit previewHit;
        previewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(previewRay, out previewHit, 100.0f)){
            if (previewHit.collider.name == "Space"){
                previewObj = Instantiate(planets[selectedPlanet], previewHit.point, Quaternion.identity);
                followPreview = true;
            }
        }
    }

    public void pauseGame(){
        pauseObj = Instantiate(pausePrefab);
    }

    public void resumeGame(){
        followPreview = false;
        try {
            Destroy(previewObj);
        }catch(Exception){}
        Destroy(pauseObj); 
    }
}