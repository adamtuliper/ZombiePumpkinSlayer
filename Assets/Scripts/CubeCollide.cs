using UnityEngine;
using System.Collections;

public class CubeCollide : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        //Called when we have a physical collision
        Debug.Log(this.name + " Collided with " + collision.gameObject.tag);
    }

    void OnTriggerEnter(Collider collider)
    {
        //Called when another object comes within the trigger zone
        Debug.Log(this.name + " Triggered by " + collider.gameObject.name);
    } 
}
