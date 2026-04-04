using UnityEngine;
using static ConstValue;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;

    private float currentAttackCooldown;

    public float MoveSpeed {  get; private set; }
    public float AttackPoint {  get; private set; }
    public float AttackCooldown { get; private set; }
    public float BulletMoveSpeed {  get; private set; }
    public float BulletLifeTime { get; private set; }

    private void Awake()
    {
        if (playerData != null)
        {
            InitPlayerData(playerData);
        }
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        currentAttackCooldown = AttackCooldown;
    }

    private void Update()
    {
        currentAttackCooldown -= Time.deltaTime;

        if (currentAttackCooldown <= 0)
        {
            FireBullet();
        }
    }

    private void InitPlayerData(PlayerData playerData)
    {
        MoveSpeed = playerData.moveSpeed;
        AttackPoint = playerData.attackPoint;
        AttackCooldown = playerData.attackCooldown;
        BulletMoveSpeed = playerData.bulletMoveSpeed;
        BulletLifeTime = playerData.bulletLifeTime;
    }

    public void MoveAnimation(bool isMove, Vector2 inputDirection)
    {
        animator.SetBool(MoveAnim, isMove);
        animator.SetFloat(AxisX, inputDirection.x);
    }

    private void FireBullet()
    {
        GameObject bulletObject = ObjectPoolManager.Instance.SpawnFromPool(bulletPrefab);
        bulletObject.transform.position = bulletPoint.position;
        bulletObject.transform.rotation = Quaternion.identity;
        bulletObject.GetComponent<Bullet>().InitData(this);

        currentAttackCooldown = AttackCooldown;
    }
}
