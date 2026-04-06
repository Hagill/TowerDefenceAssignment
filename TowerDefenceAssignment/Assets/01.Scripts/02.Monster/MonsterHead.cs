using System.Collections.Generic;
using UnityEngine;

public class MonsterHead : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float connectMoveSpeed;
    [SerializeField] private float bodySpace;
    private Monster monster;

    public Transform[] Waypoints => waypoints;
    public float BodySpace => bodySpace;
    public float ConnectMoveSpeed => connectMoveSpeed;
    public Vector3 PathStartPoint => waypoints[0].position;

    private List<MonsterBody> activeBodies;

    private void Start()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void Update()
    {
        if (activeBodies != null && activeBodies.Count > 0)
        {
            MonsterBody frontBody = activeBodies[0];

            if (frontBody != null && frontBody.gameObject.activeInHierarchy)
            {
                Vector3 moveDir = frontBody.GetMoveDirection();
                transform.position = frontBody.transform.position + moveDir * bodySpace;
            }
        }
        else
        {
            ReturnToStartPoint();
        }
    }

    private void ReturnToStartPoint()
    {
        transform.position = Vector3.Lerp(transform.position, PathStartPoint, Time.deltaTime * 5f);
    }

    public void InitBodies(List<MonsterBody> bodies)
    {
        activeBodies = bodies;
    }

    public void OnBodyDead(MonsterBody deadBody)
    {
        monster.OnBodyDead(deadBody);
    }
}
