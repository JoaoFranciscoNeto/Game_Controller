using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    PlayerMovementInput input;

    public float MovePower = 5;
    public bool UseTorque = true;
    public float MaxAngularVelocity = 25;
    public float JumpPower = 2;

    public float GroundRayLength = 1f;

    private Rigidbody rigid;

	void Start () {
        rigid = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().maxAngularVelocity = MaxAngularVelocity;
	}

    public void Move(Vector3 direction, bool jump)
    {
        if (UseTorque)
            rigid.AddTorque(new Vector3(direction.z, 0, -direction.x) * MovePower);
        else
            rigid.AddForce(direction * MovePower);

        if (Physics.Raycast(transform.position, -Vector3.up, GroundRayLength) && jump)
        {
            Debug.Log(jump);
            rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }
    }
}
