using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

    public float speed = 20f;
    public float sensitivity = 10f;
    public float flySpeed = 5f;
    public bool invertXAxis = true;
    public bool invertYAxis = false;

    CharacterController player;

    public GameObject eyes;

    // forward/backward
    private float moveFB;
    // left/right
    private float moveLR;
    // up/down
    private float moveUD;

    private float rotX;
    private float rotY;

    // Use this for initialization
    void Start () {

        player = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {

        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;
        moveUD = 0;
        if (Input.GetKey("space"))
        {
            moveUD = flySpeed;
        }
        else if (Input.GetKey("left shift"))
        {
            moveUD = -flySpeed;
        }

        // Rotation only when right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            rotX = Input.GetAxis("Mouse X") * sensitivity;
            rotY = Input.GetAxis("Mouse Y") * sensitivity;
        }

        // Axis navigation
        Vector3 movement = new Vector3(moveLR, moveUD, moveFB);

        if (invertXAxis) {
            rotX = -rotX;
        }
        if (invertYAxis)
        {
            rotY = -rotY;
        }

        transform.Rotate(0, rotX, 0);

        eyes.transform.Rotate(rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

    }
}
