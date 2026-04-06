using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public float bodySpawnCooldown;
    public float firstBodyHp;
    public float bodyHpIncrease;
    public int maxBodyCount;
    public float moveSpeed;
    public float bodySpace;
}
