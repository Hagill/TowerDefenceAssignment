using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    public float MoveSpeed {  get; private set; }
    public float AttackPoint {  get; private set; }
    public float AttackCooldown { get; private set; }
    public float BulletMoveSpeed {  get; private set; }

    private void Awake()
    {
        if (playerData != null)
        {
            InitPlayerData(playerData);
        }
    }

    private void InitPlayerData(PlayerData playerData)
    {
        MoveSpeed = playerData.moveSpeed;
        AttackPoint = playerData.attackPoint;
        AttackCooldown = playerData.attackCooldown;
        BulletMoveSpeed = playerData.bulletMoveSpeed;
    }
}
