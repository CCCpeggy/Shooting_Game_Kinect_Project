using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    public AudioSource audioData;
    public Camera TPSCamera;
    public Camera FPSCamera;
    public float MovementSpeed = 10f;
    public float MouseSensitivity = 40f;
    public Animator anim;
    public AudioClip backgroundMusic;
    public AudioClip winningMusic;
    public bool Alive = true;
    public bool isWin = false;
    private Vector2 KeyboardMovement;
    private Vector2 MouseMovement;
    private float xRotation;
    private bool CamTrigger = false;
    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sound = GameObject.Find("ThirdPersonCam").GetComponent<AudioSource>();
        sound.clip = backgroundMusic;
        sound.volume = 0.5f;
        sound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimationSet();
        FootStepSound();
        if (CamTrigger)
        {
            FPSCamera.enabled = !FPSCamera.enabled;
            TPSCamera.enabled = !TPSCamera.enabled;
            CamTrigger = !CamTrigger;
        }
    }

    private void FixedUpdate()
    {
        TPSCam();
        PlayerMove();
    }

    void GetInput()
    {
        KeyboardMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MouseMovement = new Vector2(Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CamTrigger = !CamTrigger;
        }
    }

    void TPSCam()
    {

    }

    void PlayerMove()
    {
        if (!Alive || isWin) return;
        if (TPSCamera.enabled)
        {
            Vector3 CamToPlayer = TPSCamera.transform.position - transform.position;
            CamToPlayer *= -1;
            CamToPlayer.Set(CamToPlayer.x, 0f, CamToPlayer.z);
            CamToPlayer.Normalize();
            Debug.DrawLine(TPSCamera.transform.position, CamToPlayer, Color.green);

            Vector3 velocity = CamToPlayer * KeyboardMovement.y * MovementSpeed + Quaternion.Euler(0f, 90f, 0) * CamToPlayer * KeyboardMovement.x * MovementSpeed;

            if (velocity.magnitude >= 0.1f)
            {
                Quaternion dirQ = Quaternion.LookRotation(velocity);
                Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, velocity.magnitude * 4f * Time.deltaTime);
                GetComponent<Rigidbody>().MoveRotation(slerp);
            }
            velocity.y = GetComponent<Rigidbody>().velocity.y;
            GetComponent<Rigidbody>().velocity = velocity;
        }
        else
        {
            xRotation -= MouseMovement.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            FPSCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * MouseMovement.x);
            Vector3 velocity = transform.forward * KeyboardMovement.y * MovementSpeed + transform.right * KeyboardMovement.x * MovementSpeed;
            velocity.y = GetComponent<Rigidbody>().velocity.y;
            GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    void AnimationSet()
    {
        float a = KeyboardMovement.magnitude;
        if (TPSCamera.enabled) anim.SetFloat("MovingSpeed", a);
    }

    void FootStepSound()
    {
        if(KeyboardMovement.magnitude > 0.5f && !audioData.isPlaying && Alive && !isWin)
        {
            audioData.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spike" && Alive)
        {
            anim.SetTrigger("Death");
            Alive = !Alive;
        }
        else if (other.tag == "Chest")
        {
            Win();
        }
    }

    public void Respawn()
    {
        Alive = true;
        anim.SetTrigger("Respawn");
    }

    public void Win()
    {
        ChestController._instance.Open();
        anim.SetTrigger("Win");
        isWin = true;
        GameObject.Find("Canvas").GetComponent<setting_gui>().endCount();
        sound.clip = winningMusic;
        sound.loop = false;
        sound.Play();
    }
}
