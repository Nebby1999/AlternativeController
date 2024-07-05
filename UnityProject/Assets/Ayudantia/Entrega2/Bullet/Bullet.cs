using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolObject
{
    [SerializeField] private float _speed = 10;
    private Rigidbody _rigidbody;
    public void Initialize(GameObjectPool pool)
    {
        _pool = pool;
        _rigidbody.velocity = transform.up * _speed;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        CheckCameraBounds();
    }
    private void CheckCameraBounds()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
            return;
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0f || viewPos.x > 1f || viewPos.y < 0f || viewPos.y > 1f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
