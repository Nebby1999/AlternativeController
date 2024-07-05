using UnityEngine;
public class BulletFactory
{
    private readonly GameObjectPool _bulletPool;

    public BulletFactory(GameObjectPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public Bullet CreateBullet(Vector3 position, Quaternion rotation)
    {
        GameObject mineralGO = _bulletPool.SpawnObject(position, rotation);
        Bullet bullet = mineralGO.GetComponent<Bullet>();
        bullet.Initialize(_bulletPool);
        return bullet;
    }
}