using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] private float _capacity= 0.5f;
    public float Value{get; private set;}
    public bool IsActive {get; private set;}
    private void Awake()
    {
        Value = _capacity;
    }
    private void Update()
    {
        if(!IsActive && Value < _capacity) Value += Time.deltaTime;
        Value = Mathf.Clamp(Value, 0, _capacity);
    }
    public void TryDefense(bool input)
    {
        if(!input) 
        {
            if(!IsActive) IsActive = true; 
            return;
        }
        IsActive = false;
    }
    public float Defend(float damage)
    {
        float difference = Value - damage;
        Value = Mathf.Max(0, difference);
        return difference < 0 ? difference : 0;
    }
}
