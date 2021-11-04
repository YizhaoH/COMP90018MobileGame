using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ControlManager : MonoBehaviour
{
    public ToggleGroup toggleGroupInstance;
    public static ControlManager Instance;
    public string Controlname;
    public Toggle currentSelection
    {
        get{ return toggleGroupInstance.ActiveToggles().FirstOrDefault();}
    }
    public string getName()
    {
        return Controlname;
    }
    void Start()
    {
        //toggleGroupInstance = GetComponent<ToggleGroup>();
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        GameObject.DontDestroyOnLoad(this.gameObject);
        Controlname = "Joystick Control";
        //Debug.Log(currentSelection.name);
    }
    public void updateName()
    {
        Controlname = currentSelection.name;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
