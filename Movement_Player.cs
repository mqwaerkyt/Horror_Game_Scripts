using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Movement_Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CharacterController controller;
    public Vector3 playerVelocity;
    public Vector3 finalMove;
    public Transform transform_cam;
    public Transform transform_body;
    public Vector3 camera_angles;
    public Vector3 Mouse_input;
    public float sensitivity;
    public float playerSpeed;
    public float runSpeed;
    public float sneakSpeed;
    public float walkSpeed;
    public float stamina;
    public float detectionAngle;

    void Start()
    {
        sensitivity = 1;
        playerSpeed = 1;
        runSpeed = 9;
        sneakSpeed = 1;
        walkSpeed = 3;
        stamina = 5;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        //camera & body rotation
        Mouse_input = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"),0);
        Mouse_input = Mouse_input* sensitivity;
        camera_angles = camera_angles + Mouse_input;
        camera_angles = new Vector3(Mathf.Clamp(camera_angles.x, -90, 90), camera_angles.y, 0);
        transform_cam.localEulerAngles = camera_angles;
        transform_body.localEulerAngles = new Vector3(0,camera_angles.y, 0);

        // speeds
        if (Input.GetKey("left shift") == true && stamina >=0)
        {
            playerSpeed = runSpeed;
            stamina -= 1f * Time.deltaTime;
        }
        else if (Input.GetKey("left ctrl")==true)
        {
            playerSpeed = sneakSpeed;
            stamina += 1f * Time.deltaTime;
        } 
        else 
        {
            playerSpeed = walkSpeed;
            stamina += 0.5f * Time.deltaTime;
        }
        if (stamina >= 5) 
        {
            stamina = 5;
        }

        // body movement
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVector = inputVector.normalized * Time.deltaTime;
        Vector3 localForward = new Vector3 (transform_body.forward.x, 0, transform_body.forward.z) * playerSpeed * inputVector.z;
        Vector3 localStrafe = new Vector3(transform_body.right.x, 0, transform_body.right.z) * playerSpeed * inputVector.x;
        controller.Move(localForward);
        controller.Move(localStrafe);
    }   
}
