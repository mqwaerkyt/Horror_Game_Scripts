using UnityEngine;
using UnityEngine.UIElements;

public class Looking_and_Close : MonoBehaviour
{
    public Transform playerCameraTransform;
    public GameObject Object;
    public LayerMask playerMask;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        Object = GameObject.Find("playerCamera");
        playerCameraTransform = Object.transform;


    }

    // Update is called once per frame
    void Update()
    {


    }
    // check if player position is in x radius of object position
    public bool IsInRadius(float tr) 
    {
        if (Physics.CheckSphere(transform.position, tr, playerMask))
        {
            return (true);
        }
        return (false);
    }
    // check if angle player is look at transform is smaller than detection angle
    public bool IsLooking(Transform transform, float detectionAngle)
    {
        Vector3 v1 = -playerCameraTransform.position + transform.position;

        float angle = Vector3.Angle(playerCameraTransform.forward, v1);

        if (angle <= detectionAngle)
        {
            //print("Player_is looking");

            return (true);
        }
        else
        {
            return (false);
        }
    }
}
