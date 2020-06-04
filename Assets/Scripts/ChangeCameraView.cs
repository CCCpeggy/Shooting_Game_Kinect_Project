using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCameraView : MonoBehaviour
{
    public Camera TPSCamera;
    public Camera FPSCamera;
    public GameObject Cinemachine;
    public Image aimImage;

    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToFPSAnimation()
    {
        if (_animator != null)
            _animator.SetBool("ChangeToFPS", true);
    }

    public void ChangeToFPS()
    {
        TPSCamera.enabled = false;
        FPSCamera.enabled = true;
        Cinemachine.GetComponent<CinemachineFreeLook>().enabled = false;
        aimImage.enabled = true;
    }
    
    public void ChangeToTPSAnimation()
    {
        if (_animator != null)
            _animator.SetBool("ChangeToFPS", false);
    }
    
    public void ChangeToTPS()
    {
        TPSCamera.enabled = true;
        FPSCamera.enabled = false;
        Cinemachine.GetComponent<CinemachineFreeLook>().enabled = true;
        aimImage.enabled = false;
    }
}
