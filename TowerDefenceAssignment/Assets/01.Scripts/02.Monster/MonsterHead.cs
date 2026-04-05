using UnityEngine;
using System.Collections.Generic;

public class MonsterHead : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float connectMoveSpeed;
    private float moveSpeed;

    private int currentWaypointIndex = 0;

    public Vector3 PathStartPoint => waypoints[0].position;
    public int CurrentWaypointIndex => currentWaypointIndex;
    public Transform[] WayPoints => waypoints;
    public bool IsReverse { get; private set; } = false;
    public bool IsConnected { get; private set; } = false;
    private Vector3 connectTargetPostion;


    private void Start()
    {
        moveSpeed = GetComponentInParent<Monster>().MoveSpeed;
    }

    private void Update()
    {
        if (IsReverse)
        {
            MoveReverse();
        }
        else
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        if (CurrentWaypointIndex >= waypoints.Length) return;

        Transform target = waypoints[CurrentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWaypointIndex++;
        }
    }

    private void MoveReverse()
    {
        if (Vector3.Distance(transform.position, connectTargetPostion) < 0.1f)
        {
            IsConnected = true;
            IsReverse = false;
            return;
        }

        if (currentWaypointIndex <= 0)
        {
            IsConnected = true;
            IsReverse = false;
            return;
        }

        Transform target = waypoints[currentWaypointIndex - 1];
        transform.position = Vector3.MoveTowards(transform.position, target.position, connectMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWaypointIndex--;
        }
    }

    public void StartReverse(Vector3 targetPosition)
    {
        connectTargetPostion = targetPosition;
        IsReverse = true;
        IsConnected = false;
    }

    public void StopReverse()
    {
        IsReverse = false;
        IsConnected = false;
    }
}
