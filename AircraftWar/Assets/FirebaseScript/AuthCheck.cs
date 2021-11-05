using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainmenuContainer;
    public GameObject accountPageContainor;
    public GameObject startPageContainor;
   public GameObject settingPopoutContainor;
   public GameObject mainMenuContainor;
    void OnEnable()
    {

        if(FirebaseManager.User != null)
        {
            mainmenuContainer = GameObject.Find("MainMenuContainor");
            mainmenuContainer.transform.GetChild(0).gameObject.SetActive(true);
            updateInformation();  
            startpageUpdate(); 
           mainmenuUpdate();
            this.gameObject.SetActive(false);
        }
    }

    public void updateInformation()
    {
        accountPageContainor = GameObject.Find("AccountPageContainor");
        Transform[] allchildren = accountPageContainor.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i<allchildren.Length; i++)
        {
            Debug.Log(allchildren[i].name);
            switch (allchildren[i].name) 
            {
                case "Username":
                    allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.User.DisplayName;
                    Debug.Log("username:" + FirebaseManager.User.DisplayName);
                    break;
                case "Email":
                    allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.User.Email;
                    break;
                case "City":
                    allchildren[i].GetComponent<TMP_Text>().text = GPSLocation.Instance.gpsloc;
                    break;
                case "HighestScore":
                    allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.Instance.highestScoretext;
                    break;
                case "Logout":
                    allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SignOutButton);
                    break;
                
            }
  
        }

    }
    public void mainmenuUpdate()
    {
        mainMenuContainor = GameObject.Find("MainMenuContainor");
        Transform[] allchildren = mainMenuContainor.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i<allchildren.Length; i++)
        {
            Debug.Log(allchildren[i].name);
            if (allchildren[i].name == "Start")
            {
                allchildren[i].GetComponent<Button>().onClick.AddListener(controlSettingUpdate);
                
            }
            if (allchildren[i].name == "Exit")
            {
                allchildren[i].GetComponent<Button>().onClick.AddListener(Application.Quit);
            }
        }
    }
    public void startpageUpdate()
    {
        startPageContainor = GameObject.Find("StartPageContainor");
        Transform[] allchildren = startPageContainor.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i<allchildren.Length; i++)
        {
            Debug.Log(allchildren[i].name);
            switch (allchildren[i].name) 
            {
                case "Login":
                    allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.LoginButton);
                    break;
                case "ToRegisterPage":
                    allchildren[i].GetComponent<Button>().onClick.AddListener(UIManager.instance.RegisterScreen);
                    break;
                case "Register":
                    allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.RegisterButton);
                    break;
            }
  
        }
    }

    public void controlSettingUpdate()
    {
        settingPopoutContainor = GameObject.Find("SettingPopoutContainor");
        Transform[] allchildren = settingPopoutContainor.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i<allchildren.Length; i++)
        {
            Debug.Log(allchildren[i].name);
            if (allchildren[i].name =="Gravity Control" && allchildren[i].GetComponent<Toggle>().isOn) 
            {
                Debug.Log("switch control method");
                ControlManager.Instance.Controlname ="Gravity Control";   
            }
            else if(allchildren[i].name =="Joystick Control" && allchildren[i].GetComponent<Toggle>().isOn)
            {
                ControlManager.Instance.Controlname ="Joystick Control";   

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
