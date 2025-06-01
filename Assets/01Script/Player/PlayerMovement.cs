using System;
using _01Script.Player;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float runSpeed = 15f;
    [Header("Need")]
    [SerializeField] private PlayerInputSO _input;
    
    private float gravity = -9.8f;
    private bool IsGround => _controller.isGrounded;
    private float _verticalVelocity;
    private Vector3 _velocity;
    private Vector3 _movment;
    private CharacterController _controller;

    private void OnEnable()
    {
        _controller = GetComponent<CharacterController>();
        _input.onMovement += Move;
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -0.03f;
        }
        else
        {
            _verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        _velocity = _movment;
        _velocity *= moveSpeed;
        _velocity.y = _verticalVelocity;
        
    }

    private void Move(Vector2 move)
    {
        _movment= new Vector3(move.x, 0, move.y);
    }

    private void OnDisable()
    {
        _input.onMovement -= Move;
    }
}
