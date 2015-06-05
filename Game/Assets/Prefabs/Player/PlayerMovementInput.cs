using UnityEngine;
using System.Collections;

public class PlayerMovementInput : MonoBehaviour
{

    private bool jump;
    private Vector3 move;

    private PlayerMovement movement;

    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogError("PlayerMovement Script not found");
        }

        if (Camera.main != null)
            cam = Camera.main.transform;
        else
            Debug.LogWarning("No main camera found");

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        jump = Input.GetButton("Jump");

        if (cam != null)
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = (v * camForward + h * cam.right).normalized;
        }
        else
        {
            move = (v * Vector3.forward + h * Vector3.right).normalized;
        }
    }

    private void FixedUpdate()
    {
        movement.Move(move, jump);
        jump = false;
    }
}
