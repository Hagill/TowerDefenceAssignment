using UnityEngine;

public class MonsterBodyPart : MonoBehaviour
{
    private Transform targetTransform;
    private float space;
    private MonsterBody parentMonsterBody;

    private void Awake()
    {
        parentMonsterBody = GetComponentInParent<MonsterBody>();
    }

    public void Init(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (targetTransform == null) return;

        float dist = Vector3.Distance(transform.position, targetTransform.position);
        if (dist > space)
        {
            Vector3 dir = (transform.position - targetTransform.position).normalized;
            transform.position = targetTransform.position + dir * space;
        }
    }

    public void TakeDamage(float damage)
    {
        parentMonsterBody.TakeDamage(damage);
    }
}
