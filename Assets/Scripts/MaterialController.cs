using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public Material[] materialQuality = new Material[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToCamera = (Camera.main.transform.position - this.transform.position).magnitude;
        if(distanceToCamera <= 5){
            this.gameObject.GetComponent<MeshRenderer>().material = materialQuality[2];
        }else if(distanceToCamera > 5 && distanceToCamera <= 15){
            this.gameObject.GetComponent<MeshRenderer>().material = materialQuality[1];
        }else if(distanceToCamera > 15){
            this.gameObject.GetComponent<MeshRenderer>().material = materialQuality[0];
        }
    }
}
