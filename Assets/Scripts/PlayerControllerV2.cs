using UnityEngine;
using Cinemachine;
using AudioSource = UnityEngine.AudioSource;

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
    public GameObject Cinemachine;
    public bool isStopped;
    public GameObject[] StopArea = new GameObject[4];
    public GameObject respawnHint;
    public Transform firePoint;
    public GameObject bullet;
    public static int Kill = 0;
    
    private Vector2 Movement;
    private float xRotation;
    private bool CamTrigger = false;
    private AudioSource sound;
    private float Rotation;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        sound = GameObject.Find("ThirdPersonCam").GetComponent<AudioSource>();
        sound.clip = backgroundMusic;
        sound.volume = 0.5f;
        sound.Play();
        Movement = new Vector2(1, 0);
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
        Cursor.lockState = CursorLockMode.Locked;


        isStopped = false;
        for (int i = 0; i < 4; i++)
        {
            if (StopArea[i].GetComponent<StopArea>().isShooting) isStopped = true;
        }
        
        // Computer Control
        if (isStopped)
        {
            FindObjectOfType<ChangeCameraView>().ChangeToFPSAnimation();

            float temp = Input.GetAxis("Mouse X") * 5;
            transform.Rotate(Vector3.up * temp);
            temp = Input.GetAxis("Mouse Y") * -2;
            FPSCamera.gameObject.transform.Rotate(Vector3.right * temp);
        }
        else
        {
            FindObjectOfType<ChangeCameraView>().ChangeToTPSAnimation();
        }
        if (isStopped && Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
    }

    public void Shooting()
    {
        Vector3 begin;
        Camera camera;
        if (TPSCamera.enabled)
        {
            begin = transform.position;
            begin = TPSCamera.transform.position;
            camera = TPSCamera;
        }
        else
        {
            begin = FPSCamera.transform.position;
            camera = FPSCamera;
        }
        Vector3 targetPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Ray ray = camera.ScreenPointToRay(targetPoint);
        RaycastHit hit;// = Physics.RaycastAll(ray, 50, 13);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f, true);

            Instantiate(bullet, firePoint.position, Quaternion.identity)
                .gameObject.GetComponent<BulletControl>().target = hit.point;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CamTrigger = !CamTrigger;
        }
    }

    void PlayerMove()
    {
        if (!Alive || isWin) return;
        if (isStopped)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            return;
        }
        Vector3 velocity = (transform.forward * Movement.x * MovementSpeed + transform.right * Movement.y * MovementSpeed);
        if (TPSCamera.enabled)
        {
            Vector3 CamToPlayer = TPSCamera.transform.position - transform.position;
            CamToPlayer *= -1;
            CamToPlayer.Set(CamToPlayer.x, 0f, CamToPlayer.z);
            CamToPlayer.Normalize();
            Debug.DrawLine(TPSCamera.transform.position, CamToPlayer, Color.green);
            // velocity = CamToPlayer * Movement.y * MovementSpeed + Quaternion.Euler(0f, 90f, 0) * CamToPlayer * Movement.x * MovementSpeed;
        }
        if (velocity.magnitude >= 0.1f)
        {
            Quaternion dirQ = Quaternion.Euler(new Vector3(0, Rotation, 0));
            //Debug.Log(Rotation + ", " + dirQ);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, velocity.magnitude * 4f * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(slerp);
        }
        velocity.y = GetComponent<Rigidbody>().velocity.y;
        GetComponent<Rigidbody>().velocity = velocity;
        // if (TPSCamera.enabled)
        // {
        //     Vector3 CamToPlayer = TPSCamera.transform.position - transform.position;
        //     CamToPlayer *= -1;
        //     CamToPlayer.Set(CamToPlayer.x, 0f, CamToPlayer.z);
        //     CamToPlayer.Normalize();
        //     Debug.DrawLine(TPSCamera.transform.position, CamToPlayer, Color.green);
        // 
        //     Vector3 velocity = CamToPlayer * KeyboardMovement.y * MovementSpeed + Quaternion.Euler(0f, 90f, 0) * CamToPlayer * KeyboardMovement.x * MovementSpeed;
        // 
        //     if (velocity.magnitude >= 0.1f)
        //     {
        //         Quaternion dirQ = Quaternion.LookRotation(velocity);
        //         Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, velocity.magnitude * 4f * Time.deltaTime);
        //         GetComponent<Rigidbody>().MoveRotation(slerp);
        //     }
        //     velocity.y = GetComponent<Rigidbody>().velocity.y;
        //     GetComponent<Rigidbody>().velocity = velocity;
        // }
        // else
        // {
        //     xRotation -= MouseMovement.y;
        //     xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // 
        //     FPSCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //     transform.Rotate(Vector3.up * MouseMovement.x);
        //     Vector3 velocity = transform.forward * KeyboardMovement.y * MovementSpeed + transform.right * KeyboardMovement.x * MovementSpeed;
        //     velocity.y = GetComponent<Rigidbody>().velocity.y;
        //     GetComponent<Rigidbody>().velocity = velocity;
        // }
    }

    void AnimationSet()
    {
        float a = isStopped ? 0 : Movement.magnitude;
        //if (TPSCamera.enabled) anim.SetFloat("MovingSpeed", a);
        anim.SetFloat("MovingSpeed", a);
    }

    void FootStepSound()
    {
        if(Movement.magnitude > 0.5f && !audioData.isPlaying && Alive && !isWin && !isStopped)
        {
            audioData.Play();
        }
    }

    public void Dead()
    {
        anim.SetTrigger("Death");
        Alive = !Alive;
    }

    public void ShowRespawnHint()
    {
        if (respawnHint != null)
            respawnHint.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chest")
        {
            Win();
        }
        else if (other.tag == "MovementPosX")
        {
            Rotation = 90;
        }
        else if (other.tag == "MovementNegX")
        {
            Rotation = -90;
        }
        else if (other.tag == "MovementPosY")
        {
            Rotation = 0;
        }
        else if (other.tag == "MovementNegY")
        {
            Rotation = 180;
        }
        else if (other.tag == "Plank")
        {
            Dead();
        }
        else if (other.tag == "StopArea")
        {
            other.GetComponent<StopArea>().SpawnEnemy();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "StopArea")
        {
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
