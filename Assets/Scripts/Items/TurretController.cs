using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform spawnPoint = null;

    private ObjectPool objectPool;

    private void Start()
    {
        objectPool = ObjectPool.Instance;

        InvokeRepeating(nameof(Shoot), fireRate, fireRate);
    }

    private void Shoot()
    {
        objectPool.GetFromPool(ObjectToSpawnType.Bullet, spawnPoint.position, transform.localRotation, Vector3.one / 4);
    }
}
