using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    //[SerializeField] PlayerMovement playerMovement;
    //void Start()
    //{
    //    playerMovement = GetComponent<PlayerMovement>();
    //}

    void Update()
    {
        transform.position = cameraPosition.position;
        //if(playerMovement.isSliding)
        //{
        //    transform.position = new Vector3(0, cameraPosition.position.y/2, 0);
        //}
        //else transform.position = cameraPosition.position;
    }
}