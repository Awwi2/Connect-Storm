using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform respawn;
    [SerializeField] Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.transform.position = respawn.position;
            other.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }

}
