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
        startpageUpdate(); 
        accountPageUpdate(); 
        mainmenuUpdate();
        if(FirebaseManager.User != null)
        {
            mainmenuContainer = GameObject.Find("MainMenuContainor");
            mainmenuContainer.transform.GetChild(0).gameObject.SetActive(true);
            
            this.gameObject.SetActive(false);
        }
    }

    public void accountPageUpdate()
    {
        accountPageContainor = GameObject.Find("AccountPageContainor");
        Transform[] allchildren = accountPageContainor.transform.GetComponentsInChildren<Transform>(true);
        if(FirebaseManager.User !=null)
        {
            for (int i = 0; i<allchildren.Length; i++)
            {
                //Debug.Log(allchildren[i].name);
                switch (allchildren[i].name) 
                {
                    case "Username":
                        allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.User.DisplayName;
                        //Debug.Log("username:" + FirebaseManager.User.DisplayName);
                        break;
                    case "Email":
                        allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.User.Email;
                        break;
                    case "City":
                        allchildren[i].GetComponent<TMP_Text>().text = GPSLocation.Instance.gpsloc;
                        break;
                    case "UsernameField":
                        if(FirebaseManager.Instance.usernameField==null)
                        FirebaseManager.Instance.usernameField = allchildren[i].GetComponent<TMP_InputField>();
                        break;
                    
                    case "HighestScore":
                        allchildren[i].GetComponent<TMP_Text>().text = FirebaseManager.Instance.highestScoretext;
                        break;
                    case "Logout":
                        allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SignOutButton);
                        break;  
                    case "SaveChange":
                        allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SaveChange);
                        break;    
                }
            }
        }else
        {
            for (int i = 0; i<allchildren.Length; i++)
            {
                //Debug.Log(allchildren[i].name);
                switch (allchildren[i].name) 
                {
                    case "Username":
                        if(FirebaseManager.Instance.usernameText==null)
                        FirebaseManager.Instance.usernameText = allchildren[i].GetComponent<TMP_Text>();
                        break;
                    case "Email":
                        if(FirebaseManager.Instance.EmailText==null)
                        FirebaseManager.Instance.EmailText = allchildren[i].GetComponent<TMP_Text>();
                        break;
                    case "UsernameField":
                        if(FirebaseManager.Instance.usernameField==null)
                        FirebaseManager.Instance.usernameField = allchildren[i].GetComponent<TMP_InputField>();
                        break;

                    case "City":
                        if(FirebaseManager.Instance.City==null)
                        FirebaseManager.Instance.City = allchildren[i].GetComponent<TMP_Text>();
                        break;
                    case "HighestScore":
                        if(FirebaseManager.Instance.HightscoreText==null)
                        FirebaseManager.Instance.HightscoreText = allchildren[i].GetComponent<TMP_Text>();
                        break;
                    case "Logout":
                        allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SignOutButton);
                        break;     
                    case "SaveChange":
                        allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SaveChange);
                        break; 
                }
            }
        }
        

    }
    public void mainmenuUpdate()
    {
        mainMenuContainor = GameObject.Find("MainMenuContainor");
        Transform[] allchildren = mainMenuContainor.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i<allchildren.Length; i++)
        {
            //Debug.Log(allchildren[i].name);
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
            //Debug.Log(allchildren[i].name);
            switch (allchildren[i].name) 
            {
                //l0gin
                case "L_AccountField":
                    FirebaseManager.Instance.emailLoginField =  allchildren[i].GetComponent<TMP_InputField>();
                    break;
                case "L_PasswordsField":
                    FirebaseManager.Instance.passwordLoginField =  allchildren[i].GetComponent<TMP_InputField>();
                    break;
                
                case "L_Warnning_Text":
                    FirebaseManager.Instance.warningLoginText = allchildren[i].GetComponent<TMP_Text>();
                    break;
                case "L_Comfirm_Text":
                    FirebaseManager.Instance.confirmLoginText = allchildren[i].GetComponent<TMP_Text>();
                    break;
                //register
                case "R_UserNameField":
                    if(FirebaseManager.Instance.usernameRegisterField==null)
                    FirebaseManager.Instance.usernameRegisterField = allchildren[i].GetComponent<TMP_InputField>();
                    break;
                case "RegisterAccountField":
                    FirebaseManager.Instance.emailRegisterField = allchildren[i].GetComponent<TMP_InputField>();
                    break;
                case "PWDField":
                    FirebaseManager.Instance.passwordRegisterField = allchildren[i].GetComponent<TMP_InputField>();
                    break;
                case "ComfirmPWDfield":
                    FirebaseManager.Instance.passwordRegisterVerifyField = allchildren[i].GetComponent<TMP_InputField>();
                    break;  
                case "R_Warnning_Text ":
                    FirebaseManager.Instance.warningRegisterText = allchildren[i].GetComponent<TMP_Text>();
                    break;
                case "Login":
                    //Debug.Log("Login embedded!!!!!!!!!!");
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
            //Debug.Log(allchildren[i].name);
            if (allchildren[i].name =="Gravity Control" && allchildren[i].GetComponent<Toggle>().isOn) 
            {
                //Debug.Log("switch control method");
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
