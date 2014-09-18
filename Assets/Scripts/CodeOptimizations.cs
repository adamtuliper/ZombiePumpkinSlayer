using UnityEngine;
using System.Collections;

public class CodeOptimizations : MonoBehaviour
{

    private Animator _animator;

    void Awake()
    {
        //Awake calls before Start. We can safely get our own components in this method
        //Other game objects though may not be initialized yet so we avoid looking for them here.

        //Note we do GetComponent<Animator> rather than GetComponent(
        _animator = this.GetComponent<Animator>();
        //Don't do this: _animator = (Animator)GetComponent(typeof (Animator));
    }


    private GameObject _player;
	// Use this for initialization
	void Start ()
	{
        //In the start method we can search for other object references
        //Ideally we would also just have a ref to the main player in a GameController (orchestrator) class
	    _player = GameObject.FindGameObjectWithTag("Player");
	    if (!_player)
	    {
	        Debug.LogError("Could not find the player");
	    }
	}
	
	// Update is called once per frame
	void Update () {

#region Cache Reference to save cross boundary perf hit from managed to native and back again
        
        
        /* Multiple uses are multiple hits 
           Cache the transform private Transform _transform;
         * Can also use a trigger or OnBecameInvisible/OnBecameInvisible to do 
         * a kind of distance check, also can use a trigger which runs in native code.
         * Below we also make the mistake of looking up a game object every loop
         */
        
        //check distance
        if (Vector3.Distance(transform.position, GameObject.Find("Zombie_Kid").transform.position) <50f)
	    {

	        var offset = transform.position.x - transform.position.sqrMagnitude;
	    }
	    if (transform.position.y > 100)
	    {
	        
	    }
#endregion

#region Avoid expensive finds
        /* checking for game objects every loop is not performant. This checks every game object until it's found */
	    var zombie = GameObject.Find("zombie_kid");
        //Find the player - only checks game objects with tags.
	    zombie = GameObject.FindGameObjectWithTag("Player");
        //Even better take the above line and do this on void Startup()
        //Also cache reference in the editor via
        //[SerializeField] private GameObject _zombieKid;
#endregion



        #region Component references - cache them if used a lot
        //careful of: this.rigidbody

	    _animator.SetBool("Running", true);

	    #endregion
	}
}
