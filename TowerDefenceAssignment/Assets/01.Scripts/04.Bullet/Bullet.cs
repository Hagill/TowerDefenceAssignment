using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject originalPrefab;
    [SerializeField] private LayerMask monsterLayer;

    private float bulletLifetime;
    private float currentLifetime;
    private float bulletMoveSpeed;
    private float bulletDamage;

    private void Start()
    {
        currentLifetime = bulletLifetime;
    }

    private void OnEnable()
    {
        currentLifetime = bulletLifetime;
    }

    private void Update()
    {
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0)
        {
            ObjectPoolManager.Instance.ReturnToPool(gameObject, originalPrefab);
        }

        transform.position += Vector3.up * bulletMoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentCollision = collision.gameObject;

        if (((1<<currentCollision.layer) & monsterLayer) != 0)
        {
            MonsterBody body = currentCollision.GetComponent<MonsterBody>();
            if (body != null)
            {
                body.TakeDamage(bulletDamage);
                ObjectPoolManager.Instance.ReturnToPool(gameObject, originalPrefab);
            }
        }
    }

    public void InitData(Player player)
    {
        bulletMoveSpeed = player.BulletMoveSpeed;
        bulletLifetime = player.BulletLifeTime;
        bulletDamage = player.AttackPoint;
    }
}
