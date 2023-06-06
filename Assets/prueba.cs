using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prueba : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.25f);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
