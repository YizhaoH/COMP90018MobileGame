using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Joystick joystick;
    public delegate void InputSpaceEventHandler(float space);
    private InputSpaceEventHandler inputSpaceEventHandler;

    public ControlManager controlManager;
    //public float initialPos = 0.0f;
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
    //private bool start = true;


    private void Awake()
    {
        Instance = this;
        controlManager = GameObject.FindWithTag("Control").GetComponent<ControlManager>();
    }

    private void Update()
    {
        //if (start)
        //{
        //  initialPos = Input.acceleration.z;
        //start = false;
        //}

        if (inputSpaceEventHandler != null)
        {
            inputSpaceEventHandler(Input.GetAxisRaw("Space"));
        }

        if (OnInputHorizontalOrVertical != null)
        {
            //OnInputHorizontalOrVertical(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   //使用键盘输入  
            if (controlManager != null)
            {
                String name = controlManager.getName();
                if(name == "Joystick Control")
                {
                    joystick.gameObject.SetActive(true);
                    OnInputHorizontalOrVertical(joystick.Horizontal, joystick.Vertical);
                }
                else
                {
                    OnInputHorizontalOrVertical(Input.acceleration.x*1.3f, -Input.acceleration.z*1.3f - 0.75f);
                    joystick.gameObject.SetActive(false);
                }
            }      
            //OnInputHorizontalOrVertical(joystick.Horizontal, joystick.Vertical); // 使用摇杆输入
            //OnInputHorizontalOrVertical(Input.acceleration.x*1.3f, -Input.acceleration.z*1.3f - 0.75f);
            //Debug.Log(Input.acceleration.z);
            //OnInputHorizontalOrVertical = null;
        }
        //if (Input.touchCount > 0) {
        //  Vector3 pos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
        //transform.position = pos;
        //}
    }
}
