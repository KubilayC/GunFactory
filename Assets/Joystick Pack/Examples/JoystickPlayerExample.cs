using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        transform.eulerAngles = new Vector3(0, Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * 180 / Mathf.PI, 0);
        rb.velocity = (Vector3.forward * 2 * variableJoystick.Vertical + Vector3.right *2* variableJoystick.Horizontal);

    }
}