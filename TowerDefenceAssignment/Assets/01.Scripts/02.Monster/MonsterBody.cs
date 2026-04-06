using UnityEngine;
using TMPro;

public class MonsterBody : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;

    private MonsterHead monsterHead;
    private bool isActive;
    private bool isMoving;
    private bool isReversing;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private float moveSpeed;
    private float connectMoveSpeed;
    private float reverseDistanceRemaining;

    private float maxHp;
    private float currentHp;

    public bool IsConnected { get; private set; } = false;

    public void Init(MonsterHead head, Transform[] waypoints, float moveSpeed, float connectMoveSpeed, float hp)
    {
        monsterHead = head;
        this.waypoints = waypoints;
        this.moveSpeed = moveSpeed;
        this.connectMoveSpeed = connectMoveSpeed;
        currentWaypointIndex = 0;
        reverseDistanceRemaining = 0;
        isActive = true;
        isMoving = true;
        isReversing = false;
        IsConnected = false;

        maxHp = hp;
        currentHp = maxHp;
        UpdateHpText();
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    public void StartReconnect()
    {
        isReversing = true;
        isMoving = false;
        IsConnected = false;
    }

    private void Update()
    {
        if (!isActive) return;

        if (isReversing)
            MoveReverse();
        else if (isMoving)
            MoveForward();
    }

    private void MoveForward()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length) return;

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (transform.position == target.position)
            currentWaypointIndex++;
    }

    private void MoveReverse()
    {
        if (reverseDistanceRemaining <= 0f || currentWaypointIndex == 0)
        {
            IsConnected = true;
            isReversing = false;
            isMoving = true;
            return;
        }

        Vector3 prevWaypoint = waypoints[currentWaypointIndex - 1].position;
        float step = Mathf.Min(connectMoveSpeed * Time.deltaTime, reverseDistanceRemaining);
        float distToPrev = Vector3.Distance(transform.position, prevWaypoint);

        if (distToPrev <= step)
        {
            reverseDistanceRemaining -= distToPrev;
            transform.position = prevWaypoint;
            currentWaypointIndex--;
        }
        else
        {
            reverseDistanceRemaining -= step;
            transform.position = Vector3.MoveTowards(transform.position, prevWaypoint, step);
        }

        if (reverseDistanceRemaining <= 0f)
        {
            IsConnected = true;
            isReversing = false;
            isMoving = true;
        }
    }

    public Vector3 GetMoveDirection()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length)
        {
            return Vector3.up;
        }

        return (waypoints[currentWaypointIndex].position - transform.position).normalized;
    }

    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(active);
    }

    public void StartReverse(float distance)
    {
        reverseDistanceRemaining = distance;
        isReversing = true;
        isMoving = false;
        IsConnected = false;
    }

    private void UpdateHpText()
    {
        if (hpText != null)
        {
            hpText.text = Mathf.CeilToInt(currentHp).ToString();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        UpdateHpText();

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isActive = false;
        monsterHead.OnBodyDead(this);
    }
}
