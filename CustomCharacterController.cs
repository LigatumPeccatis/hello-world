using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(CameraMover))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CustomCharacterController : MonoBehaviour {

    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    float currentSpeed;

    [SerializeField]
    Vector3 velocity = Vector3.zero;
    

    [SerializeField]
    float rotatingSpeed;
    
    [SerializeField]
    float jumpForce;

    [SerializeField]
    float acceleration;
    float testRot = 0f;


    Animator anim;
    Mover mover;
    Detection detector;
    Interactor interator;
    Skills skillController;
    CameraMover camMover;
    // Use this for initialization
    void Start () {
        mover = GetComponent<Mover>();
        camMover = GetComponent<CameraMover>();
        detector = GetComponent<Detection>();
        interator = GetComponent<Interactor>();
        anim = GetComponent<Animator>();
        skillController = GetComponent<Skills>();

        testRot = transform.eulerAngles.x;
    }
	
	// Update is called once per frame
	void Update () {
        speedChange();
        moving();
        jumping();
        //rotating();
        skillImplementator();

         Quaternion rotation = Quaternion.LookRotation(camMover.camDirection, Vector3.up);
        float angle = Quaternion.Angle(rotation, transform.rotation);
        Debug.Log("angle: "+angle);
    }

    void moving() {
        float movingY = Input.GetAxisRaw("Vertical");
        float movingX = Input.GetAxisRaw("Horizontal");
        Vector3 newPosY = transform.forward * movingY;
        Vector3 newPosX = transform.right * movingX;
        Vector3 newPos = (newPosY + newPosX).normalized * currentSpeed;
        velocity = new Vector3(Mathf.Lerp(velocity.x, newPos.x, .2f), 0, Mathf.Lerp(velocity.z, newPos.z, .2f) );
        float movingSpeedPercent = velocity.magnitude/maxSpeed;
        if (newPos != Vector3.zero)
        {
            mover.Move(velocity);
            //rotating();
        }
        anim.SetFloat("Moving", movingSpeedPercent);
        
    }

    void jumping(){
        if (Input.GetButtonDown("Jump")) {
           
            mover.Jump(jumpForce);
        }
    }

    void speedChange(){
       
        if(Input.GetButton("Run")){
            Debug.Log("Run");
            currentSpeed = maxSpeed;
        }else{
            Debug.Log("Walk");
            currentSpeed = walkSpeed;
        }
    }



    void rotating() {

        //float rotX = Input.GetAxisRaw("Mouse X");
        //float rotY = Input.GetAxisRaw("Mouse Y");
        //Vector3 vectorRotCam = new Vector3(rotY, -rotX, 0) * rotatingSpeed;
        //float angle = 0.0f;
        //Vector3 objAxis = Vector3.zero;
        //transform.rotation.ToAngleAxis(out angle, out objAxis);
        Quaternion rotation = Quaternion.LookRotation(camMover.camDirection, Vector3.up);
        float angle = Quaternion.Angle(rotation, transform.rotation);
        Debug.Log("angle: "+angle);
        if(angle > 5){
	    /*
            testRot+=angle;
            float dotVectors = Vector3.Dot(camMover.camDirection, transform.forward.normalized);
            float rotAngle = Mathf.Acos(dotVectors);
            Vector3 EulerRot = new Vector3(0, rotAngle, 0);
            Vector3 vectorRot = new Vector3(0, rotX, 0) * rotatingSpeed;
            Vector3 vectorRot = new Vector3();
            Debug.Log("EulerRot: " + EulerRot);
            Quaternion start = transform.rotation;
            Quaternion disierRot = Quaternion.Euler(new Vector3 (0, testRot, 0) );
            Quaternion destination = start * Quaternion.Euler(EulerRot); */
            //if(EulerRot != Vector3.zero){
                Quaternion rot =  Quaternion.Lerp(start, rotation, Time.deltaTime * .1f); //Quaternion.Euler(EulerRot);
                //Quaternion rot = destination;//Quaternion.Slerp(start, destination);
                transform.rotation = rot;
                //mover.Rotate(rot);
            
        }
        //camMover.moveAround(rotX * rotatingSpeed, rotY * rotatingSpeed);
        //Debug.Log(rotX+" | "+ rotY);

    }

    void skillImplementator() {
        if (Input.GetButtonDown("Fire1")) {
            if (detector.hasObject)
            {
                interator.Grab(detector.GetLookingObj());
            }
        }
        if (Input.GetButtonDown("Fire2")) {
            interator.Drop();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            anim.SetTrigger("MassMagick");
            
            Vector3 objPos = detector.CameraRayDetection();
            Debug.Log(objPos);
            if (objPos != new Vector3()) {
                
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            anim.SetTrigger("ThrowArroungObjects");
            
            Vector3 objPos = detector.CameraRayDetection();
          
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Quaternion rotation = Quaternion.LookRotation(camMover.camDirection, Vector3.up);
            //float angle = Quaternion.Angle(rotation, transform.rotation);
            transform.rotation = rotation;
            //Debug.Log("angle: "+angle);
        }

    }

    public void ForceHit(){
        skillController.throwForce();
        Debug.Log("ForceHitEvent");
    }

}
