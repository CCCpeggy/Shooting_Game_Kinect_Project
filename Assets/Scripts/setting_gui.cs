using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class setting_gui : MonoBehaviour
{
    public bool pauseEnabled = false;
    public bool menuShow = false;
    public bool menuStorePauseState = false;
    private bool miniMapShow = false;
    private bool skyboxShow = false;
    private bool timeShow = true;

    private bool doCountDown = false;
    private bool doCount = true;
    private float startCountDownTime = 0;
    private float startCountTime = 0;
    private float currentCountTime = 0;
    private float countDownTime = 0;

    private GameObject miniMap;
    private GameObject settingMenu;
    private GameObject TimeText;

    void Start()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
        miniMap = gameObject.transform.GetChild(0).gameObject;
        settingMenu = gameObject.transform.GetChild(1).gameObject;
        TimeText = gameObject.transform.GetChild(2).gameObject;
        //startCountDown(15.0f, demoTime, 10);
        startCount();
        showUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //check if game is already paused
            if (menuShow)
            {
                //unpause the game
                pauseEnabled = false;
                pauseEnabled = menuStorePauseState;
            }
            //else if game isn't paused, then pause it
            else
            {
                menuStorePauseState = pauseEnabled;
                pauseEnabled = true;
            }
            menuShow = !menuShow;
        }
        if (pauseEnabled)
        {
            AudioListener.volume = 0;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            showUI();
        }
        else
        {
            //unpause the game
            Time.timeScale = 1;
            AudioListener.volume = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            showUI();
        }
        if (doCountDown && !menuShow && !pauseEnabled)
        {
            float remainTime = countDownTime - (Time.time - startCountDownTime);
            if (remainTime < 0.02)
            {
                doCountDown = false;
                remainTime = 0;
            }
            int mm = (int)remainTime / 60;
            int ss = (int)remainTime % 60;
            int sss = (int)(remainTime * 100) - (mm * 60 + ss) * 100;
            TimeText.GetComponent<Text>().text = mm+":"+ss+"'"+sss;
            if (!doCountDown)
            {
                showUI();
            }
        }
        else if (!menuShow && !pauseEnabled)
        {
            if (doCount) currentCountTime = Time.time;
            float passTime = (currentCountTime - startCountTime);
            int mm = (int)passTime / 60;
            int ss = (int)passTime % 60;
            int sss = (int)(passTime * 100) - (mm * 60 + ss) * 100;
            TimeText.GetComponent<Text>().text = mm + ":" + ss + "'" + sss;
        }
    }
    void showUI()
    {
        if (menuShow)
        {
            if (miniMapShow) settingMenu.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "關閉小地圖";
            else settingMenu.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "開啟小地圖";
            if (timeShow) settingMenu.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "關閉計時器";
            else settingMenu.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "開啟計時器";
            if (skyboxShow) settingMenu.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "關閉天空盒";
            else settingMenu.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "開啟天空盒";
        }

        miniMap.SetActive(!menuShow && miniMapShow);
        settingMenu.SetActive(menuShow);
        TimeText.SetActive(!menuShow && timeShow);
    }
    public void onMiniMapBtn()
    {
        miniMapShow = !miniMapShow;
        showUI();
    }
    public void onSkyboxBtn()
    {
        skyboxShow = !skyboxShow;
        if (skyboxShow)
        {
            GameObject.Find("ThirdPersonCam").GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            GameObject.Find("FirstPersonCam").GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;  
        }
        else
        {
            GameObject.Find("ThirdPersonCam").GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            GameObject.Find("FirstPersonCam").GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        }
        showUI();
    }
    public void onTimeBtn()
    {
        timeShow = !timeShow;
        showUI();
    }
    public void startCountDown(float time)
    {
        startCountDownTime = Time.time;
        countDownTime = time;
        doCountDown = true;
        showUI();
        //method.DynamicInvoke(args);
    }
    private void demoTime(int i)
    {
        Debug.Log(i);
    }
    public void startCount()
    {
        startCountTime = Time.time;
        doCount = true;
        showUI();
    }
    public void endCount()
    {
        doCount = false;
    }
}