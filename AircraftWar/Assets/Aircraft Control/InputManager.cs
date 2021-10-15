using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Joystick joystick;
    public delegate void InputSpaceEventHandler(float space);
    private InputSpaceEventHandler inputSpaceEventHandler;    
    public event InputSpaceEventHandler OnInputSpace 
        {
            add 
            {
                inputSpaceEventHandler += value;
            }
            remove 
            {
                inputSpaceEventHandler -= value;
            }
        }
    public event Action<float, float> OnInputHorizontalOrVertical;

    private void Awake() 
    {
        Instance = this;
    }

    private void Update()
    {
        if (inputSpaceEventHandler != null) {
            inputSpaceEventHandler(Input.GetAxisRaw("Space"));
        }

        if (OnInputHorizontalOrVertical != null) {
            //OnInputHorizontalOrVertical(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));            
           OnInputHorizontalOrVertical(joystick.Horizontal, joystick.Vertical);
           
               //OnInputHorizontalOrVertical = null;
        }
        //if (Input.touchCount > 0) {
          //  Vector3 pos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            //transform.position = pos;
        //}
    }
}
