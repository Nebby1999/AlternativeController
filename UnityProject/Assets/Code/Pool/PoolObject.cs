using UnityEngine;

public class PoolObject : MonoBehaviour, IPooleable
{
    public GameObjectPool Pool => _pool;
    [SerializeField] protected GameObjectPool _pool;
    private void OnDisable()
    {
        _pool.ReturnToPool(this.gameObject);
    }
    private void OnDestroy()
    {
        _pool.RemoveObject(this.gameObject);
    }
}
