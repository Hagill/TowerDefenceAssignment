using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterHead head;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameSceneManager gameSceneManager;

    private int activeBodyCount;

    private List<MonsterBody> allBodyList = new List<MonsterBody>();
    private List<MonsterBody> activeBodyList = new List<MonsterBody>();
    private int spawnCount;

    private Coroutine connectCoroutine;

    public float BodySpawnCooldown { get; private set; }
    public float FirstBodyHp { get; private set; }
    public float BodyHpIncrease { get; private set; }
    public int MaxBodyCount { get; private set; }
    public float MoveSpeed { get; private set; }
    public float BodySpace { get; private set; }

    private void Start()
    {
        if (monsterData != null)
        {
            InitData(monsterData);
        }

        SpawnBody();
        head.InitBodies(activeBodyList);

        StartCoroutine(ActiveBodyCoroutine());
    }

    private void InitData(MonsterData monsterData)
    {
        BodySpawnCooldown = monsterData.bodySpawnCooldown;
        FirstBodyHp = monsterData.firstBodyHp;
        BodyHpIncrease = monsterData.bodyHpIncrease;
        MaxBodyCount = monsterData.maxBodyCount;
        MoveSpeed = monsterData.moveSpeed;
        BodySpace = monsterData.bodySpace;
        spawnCount = 0;
    }

    private void SpawnBody()
    {
        for (int i = 0; i < MaxBodyCount; i++)
        {
            GameObject bodyObject = Instantiate(bodyPrefab, head.PathStartPoint, Quaternion.identity);
            MonsterBody body = bodyObject.GetComponent<MonsterBody>();

            if (body != null)
            {
                float hp = FirstBodyHp + BodyHpIncrease * i;
                body.Init(head, head.Waypoints, MoveSpeed, head.ConnectMoveSpeed, hp);
                body.SetActive(false);
                allBodyList.Add(body);
            }
        }
    }

    private IEnumerator ActiveBodyCoroutine()
    {
        while (activeBodyCount < allBodyList.Count - 1)
        {
            yield return new WaitForSeconds(BodySpawnCooldown);

            if (spawnCount >= MaxBodyCount) yield break;

            MonsterBody body = allBodyList[spawnCount];
            body.SetActive(true);

            activeBodyList.Add(body);

            spawnCount++;
        }
    }

    public void OnBodyDead(MonsterBody deadBody)
    {
        int deadIndex = activeBodyList.IndexOf(deadBody);
        if (deadIndex < 0) return;

        activeBodyList.RemoveAt(deadIndex);
        deadBody.SetActive(false);

        if (activeBodyList.Count == 0 && spawnCount >= MaxBodyCount)
        {
            OnAllMonsterDead();
            return;
        }

        if (connectCoroutine != null) StopCoroutine(connectCoroutine);
        connectCoroutine = StartCoroutine(ConnectBodyCoroutine(deadIndex));
    }

    private IEnumerator ConnectBodyCoroutine(int deadIndex)
    {
        foreach (var body in activeBodyList) body.SetMoving(false);

        if (deadIndex > 0)
        {
            for (int i = 0; i < deadIndex; i++)
            {
                activeBodyList[i].StartReverse(BodySpace);
            }

            yield return new WaitUntil(() => {
                for (int i = 0; i < deadIndex; i++)
                {
                    if (!activeBodyList[i].IsConnected) return false;
                }
                return true;
            });
        }

        foreach (var body in activeBodyList) body.SetMoving(true);
        connectCoroutine = null;
    }

    private void OnAllMonsterDead()
    {
        gameSceneManager.ShowGameClearPopup();
    }
}
