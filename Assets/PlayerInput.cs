using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    public KeyCode jumpKey;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementController.desiredDirection = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(jumpKey))
        {
            movementController.Jump();
        }
    }
}
