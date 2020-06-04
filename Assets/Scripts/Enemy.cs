using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 mForward;
    private int level = 1;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = level * Mathf.Sqrt(level) * 0.2f * Random.Range(0.8f, 1.2f);
        mForward = Vector3.forward * speed;
        Transform player = FindObjectOfType<PlayerControllerV2>().gameObject.transform;
        gameObject.transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mForward = new Vector3(Vector3.forward.x * Mathf.Cos(rotationY) + Vector3.forward.z * Mathf.Sin(rotationY), Vector3.forward.y, -Vector3.forward.x * Mathf.Sin(rotationY) + Vector3.forward.z * Mathf.Cos(rotationY));
        gameObject.transform.Translate(mForward * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControllerV2>().Dead();
        }
    }   

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }
}
