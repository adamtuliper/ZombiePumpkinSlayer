using UnityEngine;
using System.Collections;

public class CrossController : MonoBehaviour
{

    //private int _health=4;
    //private GameController _gameController;

    //void Start()
    //{
    //    _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    //    if (!_gameController)
    //    {
    //        Debug.LogError("Could not find the GameController");
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cross collided with " + collision.gameObject.name);
        var go = collision.gameObject;
        if (go.tag == "Pumpkin")
        {
            //Get the pumpkins controller and tell it to act.
            var pumpkinController = go.GetComponent<PumpkinController>();
            pumpkinController.AttackCross();
        }
    }


}
