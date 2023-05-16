using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawnerController : MonoBehaviour
{
    private GameObject previewObj;

    private GameObject pauseObj;
    public GameObject pausePrefab;

    public GameObject[] planets;
    int selectedPlanet = -1;

    bool followPreview = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Comprueba que tecla se pulsa para crear un planeta u otro
        if(Input.GetKeyDown(KeyCode.A) && GameObject.Find("Pause(Clone)") == null){
            selectedPlanet = 0;
            pauseObj = Instantiate(pausePrefab);
            CreatePreview(selectedPlanet);
        }else if(Input.GetKeyDown(KeyCode.B) && GameObject.Find("Pause(Clone)") == null){
            selectedPlanet = 1;
            pauseObj = Instantiate(pausePrefab);
            CreatePreview(selectedPlanet);
        }

        //Hace que la preview del planeta siga al cursor
        if(followPreview){
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
            Instantiate(planets[selectedPlanet], previewObj.transform.position, Quaternion.identity);
            resumeGame();
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

    void resumeGame(){
        followPreview = false;
        Destroy(previewObj);
        Destroy(pauseObj); 
    }
}