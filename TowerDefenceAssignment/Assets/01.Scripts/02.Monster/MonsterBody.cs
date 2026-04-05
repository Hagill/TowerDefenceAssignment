using UnityEngine;
using System.Collections.Generic;

public class MonsterBody : MonoBehaviour
{
    private Transform followTarget;
    private Monster parentMonster;
    private bool isActive;
    
    private float space;

    private float maxHp;
    private float currentHp;

    public Monster ParentMonster => parentMonster;

    public void Init(Monster monster, Transform target, float bodySpace, float hp)
    {
        parentMonster = monster;
        space = bodySpace;
        followTarget = target;
        isActive = true;

        maxHp = hp;
        currentHp = maxHp;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    private void Update()
    {
        if (!isActive) return;
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (followTarget == null) return;

        float dist = Vector3.Distance(transform.position, followTarget.position);
        if (dist > space)
        {
            Vector3 dir = (transform.position - followTarget.position).normalized;
            transform.position = followTarget.position + dir * space;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isActive = false;
        parentMonster.OnBodyDead(this);
    }
}
