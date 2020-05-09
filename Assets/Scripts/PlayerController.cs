using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MouseSensitivity = 100f;
    public float MovementSpeed = 10f;
    public Transform CamTransform;
    public Camera FPSCamera;
    public Camera TPSCamera;
    public Animator anim;
   

    private Vector2 MouseMovement;
    private Vector2 KeyboardMovement;
    private bool CamTrigger = false;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimationSet();
        if (CamTrigger)
        {
            if (!FPSCamera.enabled) xRotation = 0f;
            FPSCamera.enabled = !FPSCamera.enabled;
            TPSCamera.enabled = !TPSCamera.enabled;
            CamTrigger = !CamTrigger;
        }
    }

    private void FixedUpdate()
    {
        FPSCam();
        Move();
    }

    void GetInput()
    {
        MouseMovement = new Vector2(Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime);
        KeyboardMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(Input.GetKeyDown(KeyCode.F5))
        {
            CamTrigger = !CamTrigger;
        }
    }

    void FPSCam()
    {
        xRotation -= MouseMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CamTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseMovement.x);
    }

    void Move()
    {
        Vector3 velocity = transform.forward * KeyboardMovement.y * MovementSpeed + transform.right * KeyboardMovement.x * MovementSpeed;
        velocity.y = GetComponent<Rigidbody>().velocity.y;
        GetComponent<Rigidbody>().velocity = velocity;
    }

    void AnimationSet()
    {
        if (TPSCamera.enabled) anim.SetFloat("MoveDir", (KeyboardMovement.y + 1)/2);
    }
}
