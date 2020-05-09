using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject respawnHint;
    public PlayerControllerV2 pc;
    public Transform spawnLoc;
    public BoxCollider bc;
    // Update is called once per frame
    void Update()
    {
        Respawn();
        if(!pc.Alive && !respawnHint.activeSelf)
        {
            respawnHint.SetActive(true);
            GameObject.Find("Canvas").GetComponent<setting_gui>().endCount();
        }
        
    }

    void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R) && !pc.Alive)
        {
            pc.transform.position = spawnLoc.position;
            pc.Respawn();
            respawnHint.SetActive(false);
            GameObject.Find("Canvas").GetComponent<setting_gui>().startCount();
        }
    }
}
