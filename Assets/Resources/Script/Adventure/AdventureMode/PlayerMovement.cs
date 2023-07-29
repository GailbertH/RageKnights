using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody characterController;
    [SerializeField]
    private float speed;

    private PlayerAdventureActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerAdventureActions();
    }

    private void OnEnable()
    {
        inputActions.Adventure_Map.Enable();
    }

    private void OnDisable()
    {
        inputActions.Adventure_Map.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputActions.Adventure_Map.Movement.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneTransitionManager.Instance.StartTransition(TransitionKey.ADVENTURE_TO_COMBAT);
    }
}
