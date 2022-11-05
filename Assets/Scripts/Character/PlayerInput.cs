using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(JumpContoller))]
public class PlayerInput : MonoBehaviour
{
    private MovementController movementController;
    private JumpContoller jumpContoller;
    public KeyCode jumpKey;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        jumpContoller = GetComponent<JumpContoller>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        movementController.OnMovement(horizontal);

        if (Input.GetKeyDown(jumpKey))
        {
            jumpContoller.OnJump(true);
        }
        if (Input.GetKeyUp(jumpKey))
        {
            jumpContoller.OnJump(false);
        }
    }
}
