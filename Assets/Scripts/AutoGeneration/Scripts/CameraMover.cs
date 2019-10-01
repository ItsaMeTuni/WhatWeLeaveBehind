using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
public class CameraMover : MonoBehaviour {

    public float speed;
    float inputX;
    float inputZ;
    public InputMaster Controlls;

    private void Awake()
    {
        Controlls = new InputMaster();
        Controlls.Player.CameraMovement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }
    // Use this for initialization
    void Start () {
		
	}
    private void Move(Vector2 Input)
    {
        Debug.Log("PlayCamMove + " + Input.x.ToString());
        inputZ = Input.y;
        inputX = Input.x;
    }
    // Update is called once per frame
    void Update () {
           inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");

            if (inputX != 0)
                moveX();
         if (inputZ != 0)
                moveZ();
        if (Input.GetAxis("Mouse ScrollWheel") > 0 ) // forward
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - 1, 100);
        }
        else if ( Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 100);

        }

    }


        void moveZ()
        {
            transform.position += transform.up * inputZ * speed * Time.deltaTime;
        }


        void moveX()
        {
            transform.position += transform.right * inputX * speed * Time.deltaTime;
        }

}
