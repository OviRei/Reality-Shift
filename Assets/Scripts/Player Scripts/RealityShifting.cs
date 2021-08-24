using UnityEngine;

public class RealityShifting : MonoBehaviour
{
    //Variables
    [Header("Reality Shifting")]
    [SerializeField] private bool BlueObjsVisability = true;
    [SerializeField] private bool OrangeObjsVisability = true;

    [Header("Tags")]
    private GameObject[] OrangeObjsTag;
    private GameObject[] BlueObjsTag;

    //Unity Functions
    private void Start()
    {
        ToggleBlueObjsVisability();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            ToggleOrangeObjsVisability();
            ToggleBlueObjsVisability();
        }
    }

    //My Functions
    private void ToggleOrangeObjsVisability()
    {
        if(OrangeObjsVisability)
        {
            OrangeObjsVisability = false;

            OrangeObjsTag = GameObject.FindGameObjectsWithTag("OrangeTag");
            foreach(GameObject OrangeObjs in OrangeObjsTag) 
            {
                OrangeObjs.GetComponent<BoxCollider>().enabled = false;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            OrangeObjsVisability = true;

            OrangeObjsTag = GameObject.FindGameObjectsWithTag("OrangeTag");
            foreach(GameObject OrangeObjs in OrangeObjsTag) 
            {
                OrangeObjs.GetComponent<BoxCollider>().enabled = true;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = true;
            }
        }    
    }

    private void ToggleBlueObjsVisability()
    {
        if(BlueObjsVisability)
        {
            BlueObjsVisability = false;

            BlueObjsTag = GameObject.FindGameObjectsWithTag("BlueTag");
            foreach(GameObject BlueObjs in BlueObjsTag) 
            {
                BlueObjs.GetComponent<BoxCollider>().enabled = false;
                BlueObjs.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            BlueObjsVisability = true;

            BlueObjsTag = GameObject.FindGameObjectsWithTag("BlueTag");
            foreach(GameObject BlueObjs in BlueObjsTag) 
            {
                BlueObjs.GetComponent<BoxCollider>().enabled = true;
                BlueObjs.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
