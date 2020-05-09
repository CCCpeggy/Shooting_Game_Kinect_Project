using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIEvent : MonoBehaviour
{
    public void onButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}
