using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField,Range(0.5f, 1.5f)] private float _movementSpeed;
    [SerializeField,Range(1000, 1500)] private float _rotationSpeed;
    [SerializeField] private bool _canMove = true;
    private ILimitable _limitStrategy;
    private Transform _transform;
    private float _offset;
    private void Awake()
    {
        _transform = transform;
    }
    public void Configure(ILimitable limit, float offset)
    {
        _limitStrategy = limit;
        _offset = offset;
    }
    public void ChangeValues(float movement, float rotation)
    {
        _movementSpeed = movement;
        _rotationSpeed = rotation;
    }
    public void SetMoveBool(bool input)
    {
        _canMove = !input;
    }
    public void TryMovement(bool input)
    {
        if (!(_canMove && input))return;
        Vector3 newPosition = (Vector2)_transform.position + (Vector2)_transform.up * (_movementSpeed * _transform.localScale.y);
        if (_limitStrategy == null)
        {
            _transform.position = newPosition;
            return;
        }
        _transform.position = _limitStrategy.ApplyLimit(newPosition, _offset);
    }
    public void TryRotate(int rotation)
    {
        float amount = rotation * _rotationSpeed * Time.deltaTime;
        _transform.Rotate(Vector3.forward, amount);
    }
}
