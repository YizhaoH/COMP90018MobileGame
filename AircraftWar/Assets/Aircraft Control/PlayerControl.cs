using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 60;
    private float thrustInput;

    public float horizontalSpeed = 30;
    public float verticalSpeed = 15;
    private Vector3 steeringInput;

    public float leanAmount_X = 90;
    public float leanAmount_Y = 30;

    public float steeringSmoothing = 1.5f;
    private Vector3 rawInputSteering;
    private Vector3 smoothInputSteering;

    public float thrustSmoothing = 2;
    private float rawInputThrust;
    private float smoothInputThrust;
    private Rigidbody rb;
    public Transform model;
    public Material mat;
    private bool isBlinking = false;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        InputManager.Instance.OnInputSpace += InputSpaceHandler;
        InputManager.Instance.OnInputHorizontalOrVertical += InputHorizontalOrVerticalHandler;
    }

    private void Update() {
        
        InputSmoothing();
    
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInputSpace -= InputSpaceHandler;
        InputManager.Instance.OnInputHorizontalOrVertical -= InputHorizontalOrVerticalHandler;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnInputSpace -= InputSpaceHandler;
        InputManager.Instance.OnInputHorizontalOrVertical -= InputHorizontalOrVerticalHandler;
    }
                 
    private void InputHorizontalOrVerticalHandler(float arg1, float arg2) {  
        Vector2 rawInput = new Vector2(arg1,arg2);
        rawInputSteering = new Vector3(rawInput.y, 0, -rawInput.x);
    }
    
    private void InputSpaceHandler(float space) {
        rawInputThrust = space;
    }

    private void InputSmoothing() {
        smoothInputSteering = Vector3.Lerp(smoothInputSteering, rawInputSteering, Time.deltaTime * steeringSmoothing);
        steeringInput = smoothInputSteering;
        //steeringInput = new Vector3(0, 0, 0);

        smoothInputThrust = Mathf.Lerp(smoothInputThrust, rawInputThrust, Time.deltaTime * thrustSmoothing);
        thrustInput = smoothInputThrust;
    }
    private void Move() {
        rb.velocity = transform.forward * moveSpeed;
    }

    private IEnumerator BlinkRoutine(float duration)
    {
        isBlinking = true;
        var blinkObject = false;
        while (duration>0)
        {
            duration -= Time.deltaTime;
            if(duration<=0)
            {
                isBlinking = false;
                mat.color = Color.white;
                yield break;
            }
            else
            {
                blinkObject = !blinkObject;
                mat.color = blinkObject ? Color.yellow : Color.red;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    // deal with the collision with boundaries and terrain
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag=="Boundary")
        {
            Vector3 prevForward = this.transform.forward;
            Vector3 newPos = this.transform.position - prevForward * 50;
            newPos.y = 200;
            this.transform.position = newPos;
            this.transform.forward = new Vector3(-prevForward.x, 0, -prevForward.z);
            float duration = 0.08f;
            if(!isBlinking) StartCoroutine(BlinkRoutine(duration));
        }
    }

    private void Turn(){
        Vector3 newTorque = new Vector3(steeringInput.x * horizontalSpeed, -steeringInput.z * verticalSpeed, 0);
        //newTorque = new Vector3(0, -steeringInput.z * verticalSpeed, 0);
        

        rb.AddRelativeTorque(newTorque);

        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), 0.5f);
        
        TurnModel();
    }

    private void TurnModel(){
        model.localEulerAngles = new Vector3(steeringInput.x * leanAmount_Y, model.localEulerAngles.y, steeringInput.z * leanAmount_X);
        //float rotatedAngle = model.eulerAngles.x + y * roateSpeed;
        //Debug.Log(model.eulerAngles.x+" " +transform.localEulerAngles.x);
       // if (model.localEulerAngles.x > 70 && model.localEulerAngles.x < 180)
        //    model.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 90), 0.05f);
            
    }
}