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
    void OnEnable()
    {

        if(FirebaseManager.User != null)
        {
            mainmenuContainer = GameObject.Find("MainMenuContainor");
            mainmenuContainer.transform.GetChild(0).gameObject.SetActive(true);
            updateInformation();  
            startpageUpdate(); 
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
                    allchildren[i].GetComponent<TMP_Text>().text = null;
                    break;
                case "HighestScore":
                    allchildren[i].GetComponent<TMP_Text>().text = null;
                    break;
                case "Logout":
                    allchildren[i].GetComponent<Button>().onClick.AddListener(FirebaseManager.Instance.SignOutButton);
                    break;
                
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
