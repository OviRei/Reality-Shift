using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    //Unity Functions
    void Update()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
