using System;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Vector3 target;

    private Vector3 _lookAt;  

    void Start()
    {
        _lookAt = (target - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_lookAt * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plank")) {
            PlayerControllerV2.Kill++;
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

}
