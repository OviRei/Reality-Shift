using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityShifting : MonoBehaviour
{
    [Header("Object Visability")]
    [SerializeField] bool BlueObjsVisability = false;
    [SerializeField] bool OrangeObjsVisability = true;

    private GameObject[] OrangeObjsTag;
    private GameObject[] BlueObjsTag;

    private void Start()
    {
        ToggleBlueObjsVisability();
    }
    private void Update()
    {
        //Detect if the middle mouse button is pressed
        if (Input.GetMouseButtonDown(2))
        {
            ToggleOrangeObjsVisability();
            ToggleBlueObjsVisability();
        }
    }
    
    void ToggleOrangeObjsVisability()
    {
        if(!OrangeObjsVisability)
        {
            OrangeObjsVisability = true;

            OrangeObjsTag = GameObject.FindGameObjectsWithTag("OrangeTag");
            foreach(GameObject OrangeObjs in OrangeObjsTag) 
            {
                OrangeObjs.GetComponent<BoxCollider>().enabled = true;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            OrangeObjsVisability = false;

            OrangeObjsTag = GameObject.FindGameObjectsWithTag("OrangeTag");
            foreach(GameObject OrangeObjs in OrangeObjsTag) 
            {
                OrangeObjs.GetComponent<BoxCollider>().enabled = false;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void ToggleBlueObjsVisability()
    {
        if(BlueObjsVisability)
        {
            BlueObjsVisability = false;

            BlueObjsTag = GameObject.FindGameObjectsWithTag("BlueTag");
            foreach(GameObject BlueObjs in BlueObjsTag) 
            {
                BlueObjs.GetComponent<BoxCollider>().enabled = true;
                BlueObjs.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            BlueObjsVisability = true;

            BlueObjsTag = GameObject.FindGameObjectsWithTag("BlueTag");
            foreach(GameObject BlueObjs in BlueObjsTag) 
            {
                BlueObjs.GetComponent<BoxCollider>().enabled = false;
                BlueObjs.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
