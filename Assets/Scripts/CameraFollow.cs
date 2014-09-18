using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 12f;		// Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
    public float zMargin = 3f;		// Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
    public float zSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
    public Vector3 maxXYZ;		// The maximum x and y coordinates the camera can have.
    public Vector3 minXYZ;		// The minimum x and y coordinates the camera can have.
    //public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

    private Transform player;		// Reference to the player's transform.


    //something

    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("player:" + player);
    }


    bool CheckZMargin()
    {
        // Returns true if the distance between the camera and the player in the z axis is greater than the z margin.
        return Mathf.Abs(transform.position.z - player.position.z) > zMargin;
    }

    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        var check = Mathf.Abs(transform.position.y - player.position.y) > yMargin;
        Debug.Log(check);
        return check;
    }


    void Update()
    {
        TrackPlayer();
    }


    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;
        float targetZ = transform.position.z;

        if (CheckZMargin())
        {
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetZ = Mathf.Lerp(transform.position.z, player.position.z, zSmooth * Time.deltaTime);
            //Debug.Log("TargetZ:" + targetZ);
        }
        // If the player has moved beyond the x margin...
       
        if (CheckXMargin())
        {
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
            Debug.Log("TargetX:" + targetX);
        }
        // If the player has moved beyond the y margin...
        if (CheckYMargin())
        {
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
            Debug.Log("TargetY:" + targetY);
        }
        
        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        //targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        //targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
       // Debug.Log(string.Format("Clamped X Clamped Y Clamped Z {0} {1} {2}", targetX, targetY, targetZ));
        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(transform.position.x, transform.position.y, targetZ);
    }
}
