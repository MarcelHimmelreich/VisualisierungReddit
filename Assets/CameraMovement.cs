using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {


    //Camera Settings
    public GameObject target;
    public float speed = 5;
    public float sprint_speed = 10;
    public float max_distance = 5;
    public bool follow = false;

    public bool right_pressed = false;
    public bool left_pressed = false;



    //Player Movement
    public float mouse_velocity = 0;
    public float sensitivity = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1)) { right_pressed = true; }
        else { right_pressed = false; }

        if (Input.GetButton("Mouse X"))
        {
            mouse_velocity = Input.GetAxis("Mouse X");
        }
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < max_distance && follow)
            {
                MoveToTarget();
            }
            if (mouse_velocity != 0)
            {
                RotateAround();
            }
        }

	}

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void EnableFollow()
    {
        follow = true;
    }

    public void MoveToTarget()
    {
        transform.Translate(target.transform.position-transform.position *speed * Time.deltaTime);
        transform.LookAt(target.transform.position * speed/5 * Time.deltaTime);
    }

    public void RotateAround()
    {
        transform.RotateAround(target.transform.position, Vector3.up, mouse_velocity * speed * Time.deltaTime);
    }


}
