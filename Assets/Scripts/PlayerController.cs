using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float zMin = 0, zMax = 100, yMin = 0, yMax = 100;
}

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 10;

    [SerializeField]
    private Boundary _boundary;

    //States
    private bool _jumping;
    private bool _smashing;
    private bool _running;

    [SerializeField]
    private int _jumpForce=150;

    private Animator _animator;


    void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    void Start()
    {
        //Ensure this object can't leave the bounds of the spawn points.
        this._boundary.zMax = GameObject.FindGameObjectWithTag("SpawnRight").transform.position.z;
        this._boundary.zMin = GameObject.FindGameObjectWithTag("SpawnLeft").transform.position.z;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumping = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _smashing = true;
            _animator.SetTrigger("Smash" + Random.Range(1,4));
        }

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (moveHorizontal < 0)
        {

            //ensure rotation is to the right.
            transform.Rotate(new Vector3(0, 154 - transform.rotation.eulerAngles.y, 0));
            //move the parent body this.rigidbody.;
            _animator.SetBool("Run", true);
            _running = true;
        }
        else if (moveHorizontal > 0)
        {
            //ensure rotation is to the left.
            transform.Rotate(new Vector3(0, -16 - transform.rotation.eulerAngles.y, 0));
            _animator.SetBool("Run", true);
            _running = true;
        }
        else
        {
            //Center when idle after a few seconds.
          //  transform.Rotate(new Vector3(0, 65 - transform.rotation.eulerAngles.y, 0));
            _animator.SetBool("Run", false);
            _running = false;
        }

        //float moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(0f, transform.rigidbody.velocity.y, moveHorizontal * _speed);
        transform.rigidbody.velocity = movement;
        //clamp boundaries
        if (_jumping)
        {
            transform.rigidbody.AddForce(0, _jumpForce, 0);
            _jumping = false;
        }


        transform.rigidbody.position = new Vector3
        (
            transform.rigidbody.position.x,
            transform.rigidbody.position.y,
            Mathf.Clamp(transform.rigidbody.position.z, _boundary.zMin, _boundary.zMax)
            //Mathf.Clamp(rigidbody.position.z, _boundary.zMin, _boundary.zMax)
        );

        //rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
    }
}
