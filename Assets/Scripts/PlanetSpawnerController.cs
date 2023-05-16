using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        Vector3 mouseRPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetMouseButtonDown(0)){
            Instantiate(planet, mouseRPos, Quaternion.identity);
        }
    }
}