using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
public class EightDirMovement : ISpeedProvider {

    public float currentSpeed;
    public float turnSpeed;
    public float gravity = 9.81f;
    public bool disableInput = false;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private Vector3 currentMovement = Vector3.zero;
    private Vector3 up = new Vector3(0, 0, 1);
    private Vector3 right = new Vector3(1, 0, 0);

    private CharacterController controller;
    public Animator animator;

    private float fltCompTolerance = 1E-6f;
    private float minMovementDetection = 1E-4f;

    private Quaternion oldRotation = Quaternion.identity;
    private Vector3 oldPosition;
    private bool moved = false;
    private bool turned = false;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("free") && !animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            return;
        }
        // Debug.Log(transform.rotation);
        resetValues();
        updateMovement();
        updateController();
        float moveDistance = (transform.position - oldPosition).magnitude;
        //Debug.Log("Move distance: " + moveDistance);
        moved =  moveDistance > minMovementDetection;
        updateRotation();
        updateAnimation();
        // Debug.Log("Speed: "  + controller.velocity);
    }

    private void updateAnimation()
    {
        //Debug.Log("Speed: " + currentSpeed);
        animator.SetFloat("Speed", currentSpeed);
        animator.GetFloat("Speed");
        //Debug.Log("Animator speed: " + animator.GetFloat("Speed") + " Animator state in free: " + animator.GetCurrentAnimatorStateInfo(0).IsName("free"));
    }

    private void updateRotation()
    {
        // Vector3 currentMovementNormalized = currentMovement.normalized;
        //Debug.Log(currentMovementNormalized);
        //float rotation = Mathf.Atan2(currentMovement.z, currentMovement.x) - Mathf.PI / 2;
        /*
        * don't rotate while falling or when not moved
        */
        //Debug.Log(controller.isGrounded);
        if (!controller.isGrounded || !moved)
        {
            return;
        }

        //Debug.Log("RotationCalled");
        Quaternion newRotation = Quaternion.LookRotation(currentMovement, Vector3.up);
        // Debug.Log("New rotation: " + newRotation.ToString());
        //transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * rotation, 0);
        transform.rotation = Quaternion.Slerp(oldRotation, newRotation, turnSpeed *  Time.deltaTime);
    }

    private void updateController()
    {
        controller.Move(currentMovement * currentSpeed * Time.deltaTime);
        controller.Move(Vector3.up * -gravity * Time.deltaTime);
    }

    private void resetValues()
    {
        oldPosition = transform.position;
        oldRotation = transform.rotation;
    }

    private void updateMovement()
    {
        if (disableInput)
        {
            return;
        }

        Vector3 direction = Vector3.zero;

        if (Input.GetKey(upKey) == true)
        {
            direction += up;
        }

        if (Input.GetKey(downKey) == true)
        {
            direction += -up;
        }

        if (Input.GetKey(leftKey) == true)
        {
            direction += -right;
        }

        if (Input.GetKey(rightKey) == true)
        {
            direction += right;
        }

        direction.y = 0;
        bool aboutToMove = direction.sqrMagnitude - fltCompTolerance > 0;

        if (aboutToMove)
        {
            direction = direction.normalized;
            currentSpeed = speed;
        } else
        {
            direction = Vector3.zero;
            currentSpeed = 0;
        }

        currentMovement = direction;
    }

    public Vector3 getCurrentMovement()
    {
        return currentMovement;
    }

    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float speed)
    {
        if (speed < 0)
        {
            speed = 0;
        }
    }
}
