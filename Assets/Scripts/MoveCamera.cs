using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    PlayerMovement playerMovement;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //transform.position = cameraPosition.position;
        if(playerMovement.isSliding || playerMovement.isCrouching || !playerMovement.isSliding && Input.GetKey(playerMovement.slideKey)) transform.position = new Vector3(cameraPosition.position.x, cameraPosition.position.y/1.5f, cameraPosition.position.z);
        else transform.position = cameraPosition.position;
    }
}