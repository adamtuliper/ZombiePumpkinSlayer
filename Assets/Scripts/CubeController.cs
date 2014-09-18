using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        var movement = new Vector3(0f, 0f, moveHorizontal * 10);
        transform.rigidbody.velocity = movement;

	}

    void OnCollisionEnter(Collision collision)
    {
        //Called when we have a physical collision
        Debug.Log(this.name + " Collided with " + collision.gameObject.name);
        //Called when we have a physical collision
        if (collision.gameObject.tag == "EnemyCube")
        {
            Destroy(collision.gameObject);
            //increment score
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //Called when another object comes within the trigger zone
        Debug.Log(this.name + " Triggered by " + collider.gameObject.name);
    } 

}
