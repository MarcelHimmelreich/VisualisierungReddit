using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {


    //Camera Settings
    public GameObject target;
    public float speed = 5;
    public float rotation_speed = 10;
    public float sprint_speed = 10;
    public float max_distance = 5;
    public float distance = 5;
    public float distance_tolerance = 1;
    public bool follow = false;

    public bool right_pressed = false;
    public bool left_pressed = false;

    public Vector3 mouse_leftclick_position;
    public Vector3 mouse_rightclick_position;
    public Vector3 mouse_current_position;

    public Vector3 movement = new Vector3(0,0,0);
    public Vector3 rotation =  new Vector3(0,0,0);
    public Vector3 target_position = new Vector3(0,0,0);
    public Quaternion target_rotation;



    //Player Movement
    public float mouse_velocity = 0;
    public float sensitivity = 1;


	// Use this for initialization
	void Start ()
    {
        rotation = transform.rotation.eulerAngles;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            right_pressed = true;
            mouse_rightclick_position = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            mouse_current_position = Input.mousePosition;           
        }
        else if (Input.GetMouseButtonUp(1))
        {
            right_pressed = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            left_pressed = true;
            mouse_leftclick_position = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            mouse_current_position = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            left_pressed = false;
        }
        mouse_current_position = Input.mousePosition;


        if (Input.GetKey("w"))
        {
            movement.z = 1;
        }
        else if (Input.GetKey("s"))
        {
            movement.z = -1;
        }
        else
        {
            movement.z = 0;
        }

        if (Input.GetKey("a"))
        {
            movement.x = -1;
        }
        else if (Input.GetKey("d"))
        {
            movement.x = 1;
        }
        else
        {
            movement.x = 0;
        }


        if (Input.GetKey("space"))
        {
            movement.y = 1;
        }
        else if (Input.GetKey("left shift"))
        {
            movement.y = -1;
        }
        else
        {
            movement.y = 0;
        }

        if (Input.GetKeyDown("f")) { follow = !follow; }

        if (follow)
        {
            if (target != null)
            {
                
                if (Vector3.Distance(transform.position, target.transform.position) > max_distance + distance_tolerance)
                {
                    MoveToTarget(true);
                }
                else if (Vector3.Distance(transform.position, target.transform.position) < max_distance - distance_tolerance)
                {
                    MoveToTarget(false);
                }
                if (left_pressed)
                {
                    mouse_velocity = Input.GetAxis("Mouse X");
                    RotateAround(Vector3.up,mouse_velocity);
                }
                else if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s")|| Input.GetKey("d"))
                {
                    RotateAround(new Vector3(0,-movement.x,movement.z),1);
                }
                RotateToTarget();
                
                rotation = transform.eulerAngles;
            }

        }
        else
        {
            if (right_pressed)
            {
                Rotate();
            }
            Move();
        }
        

    }

    public void EnableFollow()
    {
        follow = true;
    }

    public void DisableFollow() { follow = true; }

    public void Move()
    {
        transform.Translate(movement *speed *Time.deltaTime);
    }

    public void Rotate()
    {
        rotation.y += Input.GetAxis("Mouse X") * sensitivity;
        rotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        transform.eulerAngles = rotation; 
    }

    public void MoveToTarget(bool invert)
    {
        Debug.Log("Move To Target");
        target_position = target.transform.position;
        //target_position.z -= distance;
        //target_position -= transform.position;
        Vector3 velocity = Vector3.zero;
        if (invert)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target_position, ref velocity, 0.05f);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(-transform.position, target_position,ref velocity, 0.05f);
        }

    }

    public void RotateToTarget()
    {
        target_rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = target_rotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
    }

    public void RotateAround(Vector3 direction, float rotation_velocity)
    {
        transform.RotateAround(target.transform.position, direction, rotation_velocity *rotation_speed * Time.deltaTime);
    }


}
