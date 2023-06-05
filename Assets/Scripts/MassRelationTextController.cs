using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassRelationTextController : MonoBehaviour
{
    private Text massRelationText;
    // Start is called before the first frame update
    void Start()
    {
        massRelationText = this.gameObject.GetComponent<Text>();

        //Calcula la cantidad de masa en relacion al meteorito que extinguio a los dinosaurios
        float massRelation = this.gameObject.GetComponentInParent<PlanetController>().mass / 2e15f;
        massRelationText.text = massRelation.ToString();
        if (massRelation >= 1){
            massRelationText.color = Color.red;
        }else if (massRelation < 1 && massRelation >= 0.5){
            massRelationText.color = Color.yellow;
        }else {
            massRelationText.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        massRelationText.transform.LookAt(Camera.main.transform);
        massRelationText.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
