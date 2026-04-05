using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterHead head;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameSceneManager gameSceneManager;

    private List<MonsterBody> activeBodyList = new List<MonsterBody>();
    private int spawnCount;

    public float BodySpawnCooldown { get; private set; }
    public float BodySpace { get; private set; }
    public float FirstBodyHp { get; private set; }
    public float BodyHpIncrease { get; private set; }
    public int MaxBodyCount { get; private set; }
    public float MoveSpeed { get; private set; }

    private void Start()
    {
        if (monsterData != null)
        {
            InitData(monsterData);
        }
        
        spawnCount = 0;
        StartCoroutine(SpawnBodyCoroutine());
    }

    private void InitData(MonsterData monsterData)
    {
        BodySpawnCooldown = monsterData.bodySpawnCooldown;
        BodySpace = monsterData.bodySpace;
        FirstBodyHp = monsterData.firstBodyHp;
        BodyHpIncrease = monsterData.bodyHpIncrease;
        MaxBodyCount = monsterData.maxBodyCount;
        MoveSpeed = monsterData.moveSpeed;
    }

    private IEnumerator SpawnBodyCoroutine()
    {
        while (spawnCount < MaxBodyCount)
        {
            yield return new WaitForSeconds(BodySpawnCooldown);
            SpawnBody();
        }
    }

    private void SpawnBody()
    {
        GameObject bodyObject = ObjectPoolManager.Instance.SpawnFromPool(bodyPrefab);
        MonsterBody newBody = bodyObject.GetComponent<MonsterBody>();

        if (newBody != null)
        {
            float hp = FirstBodyHp + BodyHpIncrease * spawnCount;
            spawnCount++;

            bodyObject.transform.position = head.PathStartPoint;

            Transform target = activeBodyList.Count == 0 ? head.transform : activeBodyList[activeBodyList.Count - 1].transform;

            newBody.Init(this, target, BodySpace, hp);
            activeBodyList.Add(newBody);
        }
    }

    public void OnBodyDead(MonsterBody deadBody)
    {
        int deadIndex = activeBodyList.IndexOf(deadBody);
        
        if (deadIndex < 0) return;
        
        activeBodyList.RemoveAt(deadIndex);
        ObjectPoolManager.Instance.ReturnToPool(deadBody.gameObject, bodyPrefab);

        if (activeBodyList.Count == 0 && spawnCount >= MaxBodyCount)
        {
            OnAllMonsterDead();
            return;
        }

        StartCoroutine(ConnectBodyCoroutine(deadIndex));
    }

    private IEnumerator ConnectBodyCoroutine(int index)
    {
        for (int i = index; i < activeBodyList.Count; i++)
        {
            Transform target = i == 0
                ? head.transform
                : activeBodyList[i - 1].transform;

            activeBodyList[i].SetFollowTarget(target);
        }
        if (index == 0)
        {
            Vector3 targetPos = activeBodyList[0].transform.position;
            head.StartReverse(targetPos);
            yield return new WaitUntil(() => head.IsConnected);
            head.StopReverse();
        }
    }

    private void OnAllMonsterDead()
    {
        gameSceneManager.ShowGameClearPopup();
    }
}
