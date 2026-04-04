using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject originalPrefab;
    [SerializeField] private LayerMask monsterLayer;
    
    private float bulletLifetime;
    private float currentLifetime;
    private float bulletMoveSpeed;

    private void Start()
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
            //collision.GetComponent<Monster>().TakeDamage(player.AttackPoint);
            ObjectPoolManager.Instance.ReturnToPool(gameObject, originalPrefab);
        }
    }

    public void InitData(Player player)
    {
        bulletMoveSpeed = player.BulletMoveSpeed;
        bulletLifetime = player.BulletLifeTime;
    }
}
