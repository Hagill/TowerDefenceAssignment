using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed;
    public float attackPoint;
    public float attackCooldown;
    public float bulletMoveSpeed;
}
