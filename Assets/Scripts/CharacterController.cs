﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterController : NetworkBehaviour {
    public float RotationSensivity;
    public float MovementSpeed;

    private Animator animator;
    private Rigidbody rigidbody;
    private float moveSpeed;

    // Use this for initialization
    void Start ()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
            animator.SetTrigger("Start Dance 1");
        if (Input.GetKeyUp(KeyCode.Alpha2))
            animator.SetTrigger("Start Dance 2");
        if (Input.GetKeyUp(KeyCode.Alpha3))
            animator.SetTrigger("Start Dance 3");
        if (Input.GetKeyUp(KeyCode.Space))
            animator.SetTrigger("Stop Dance");
    }
   
	// Update is called once per frame
	void FixedUpdate ()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var axisDirection = new Vector3(horizontal, 0, vertical);

        if (axisDirection.magnitude > 0)
        {
            var newRotation = Quaternion.LookRotation(axisDirection.normalized);
            rigidbody.rotation = Quaternion.LerpUnclamped(rigidbody.rotation, newRotation, RotationSensivity * Time.deltaTime);
        }

        moveSpeed = 0;
        var animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorState.IsName("Walking"))
            moveSpeed = MovementSpeed * 0.3f;
        else if (animatorState.IsName("Running"))
            moveSpeed = MovementSpeed * 0.6f;
        else if (animatorState.IsName("Sprint"))
            moveSpeed = MovementSpeed;
        rigidbody.MovePosition(transform.position + axisDirection.normalized * moveSpeed * Time.deltaTime);
        animator.SetFloat("Movement Speed", axisDirection.magnitude * 11);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().Target = gameObject;
    }
}
