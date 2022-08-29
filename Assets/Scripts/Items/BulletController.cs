using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float disableAfterTime = 5f;
    private Rigidbody rigid = null;
    private ObjectPool objectPool = null;
    private GameplayManager gameplayManager = null;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableBullet), disableAfterTime);
    }

    void Start()
    {
        objectPool = ObjectPool.Instance;
        gameplayManager = GameplayManager.Instance;

        gameplayManager.gameStateChangedEvent.AddListener(NextLevel);
    }

    void Update()
    {
        rigid.velocity = speed * 100 * Time.deltaTime * -transform.right;
    }

    private void NextLevel(GameState state)
    {
        if (state != GameState.Win && state != GameState.Lose) return;

        DisableBullet();
    }

    private void DisableBullet()
    {
        if (objectPool == null) return;

        objectPool.PutBackToPool(gameObject, ObjectToSpawnType.Bullet);
    }
}
