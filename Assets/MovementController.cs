using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Vector2 desiredDirection;
    public float maxSpeed = 5f;
    public float jumpHeight = 3f;

    private float jumpForce = 0f;

    private void Update()
    {

        float x = Mathf.Lerp(this.transform.position.x, this.transform.position.x + desiredDirection.x, Time.deltaTime * maxSpeed);
        //float y = Mathf.Lerp(this.transform.position.y, this.transform.position.y + desiredDirection.y, Time.deltaTime * maxSpeed);

        float y = Mathf.Lerp(this.transform.position.y, this.transform.position.y + jumpForce, Time.deltaTime * maxSpeed);
        jumpForce = Mathf.Lerp(jumpForce, -3, Time.deltaTime * 10f);

        this.transform.position = new Vector3(x, y); 

    }

    public void Jump()
    {
        jumpForce = jumpHeight;
    }


}
