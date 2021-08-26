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
                //OrangeObjs.GetComponent<BoxCollider>().enabled = false;
                OrangeObjs.GetComponent<Collider>().enabled = false;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = false;
                if(OrangeObjs.transform.childCount > 0) OrangeObjs.transform.GetChild(0).transform.gameObject.SetActive(false);
                if(OrangeObjs.GetComponent<Rigidbody>() != null) OrangeObjs.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else
        {
            OrangeObjsVisability = true;

            OrangeObjsTag = GameObject.FindGameObjectsWithTag("OrangeTag");
            foreach(GameObject OrangeObjs in OrangeObjsTag) 
            {
                //OrangeObjs.GetComponent<BoxCollider>().enabled = true;
                OrangeObjs.GetComponent<Collider>().enabled = true;
                OrangeObjs.GetComponent<MeshRenderer>().enabled = true;
                if(OrangeObjs.transform.childCount > 0) OrangeObjs.transform.GetChild(0).transform.gameObject.SetActive(true);
                if(OrangeObjs.GetComponent<Rigidbody>() != null) OrangeObjs.GetComponent<Rigidbody>().useGravity = true;
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
                //BlueObjs.GetComponent<BoxCollider>().enabled = false;
                BlueObjs.GetComponent<Collider>().enabled = false;
                BlueObjs.GetComponent<MeshRenderer>().enabled = false;
                if(BlueObjs.transform.childCount > 0) BlueObjs.transform.GetChild(0).transform.gameObject.SetActive(false);
                if(BlueObjs.GetComponent<Rigidbody>() != null) BlueObjs.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else
        {
            BlueObjsVisability = true;

            BlueObjsTag = GameObject.FindGameObjectsWithTag("BlueTag");
            foreach(GameObject BlueObjs in BlueObjsTag) 
            {
                //BlueObjs.GetComponent<BoxCollider>().enabled = true;
                BlueObjs.GetComponent<Collider>().enabled = true;
                BlueObjs.GetComponent<MeshRenderer>().enabled = true;
                if(BlueObjs.transform.childCount > 0) BlueObjs.transform.GetChild(0).transform.gameObject.SetActive(true);
                if(BlueObjs.GetComponent<Rigidbody>() != null) BlueObjs.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
