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
    void Start()
    {

        if(FirebaseManager.User != null)
        {
            mainmenuContainer = GameObject.Find("MainMenuContainor");
            mainmenuContainer.transform.GetChild(0).gameObject.SetActive(true);
            updateInformation();   
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
            }
  
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
