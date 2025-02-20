using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlappyBird : MiniGame, IBind<FlappyBirdData>
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float minInterval;
    [SerializeField] private float maxInterval;
    [SerializeField] private float maxRandomPosY;
    [SerializeField] private float beginBlockSpeed;

    private WaitForSeconds delayInstruction;
    private FlappyBirdObstacle[] pool = new FlappyBirdObstacle[maxBudget];
    private float blockSpeed;
    private const int maxBudget = 10;

    private FlappyBirdCharacterController player;

    public BindingData<bool> IsGameOver { get; } = new(false);
    public BindingData<int> Score { get; } = new(1);
    public FlappyBirdData Data { get; private set; } = new();

    void Awake()
    {
        player = FindObjectOfType<FlappyBirdCharacterController>();
        player.isGameOver.OnValueChanged += val => IsGameOver.Value = val;
    }

    void Start()
    {
        delayInstruction = new WaitForSeconds(spawnDelay);
        SetBlockSpeed(beginBlockSpeed);

        for (int i = 0; i < maxBudget; i++)
        {
            pool[i] = GameObject.Instantiate(obstacle).GetComponent<FlappyBirdObstacle>();

            pool[i].OnUpdate += (go) =>
            {
                go.transform.localPosition += Vector3.left * blockSpeed * Time.deltaTime;

                if (go.transform.position.x < -15f)
                {
                    go.SetActive(false);
                }
            };
            pool[i].transform.SetParent(this.transform);
            pool[i].gameObject.SetActive(false);
        }

        StartCoroutine(SpawnObstacleCo());
    }

    //async task·Î ¼öÁ¤
    private IEnumerator SpawnObstacleCo()
    {
        while (!IsGameOver.Value)
        {
            var obs = pool.FirstOrDefault(x => !x.gameObject.activeSelf);

            if (obs == null)
                yield break;

            obs.Spawn(Random.Range(minInterval, maxInterval));
            obs.transform.position = transform.position + new Vector3(0f, Random.Range(-maxRandomPosY, maxRandomPosY), 0f);

            AddScore();

            yield return delayInstruction;
        }

        SaveRanking();
    }

    public void SetBlockSpeed(float blockSpeed)
    {
        this.blockSpeed = blockSpeed;
    }

    public void AddScore()
    {
        Score.Value += 1;
    }

    public void Bind(FlappyBirdData t)
    {
        Data = t;
    }

    public void SaveRanking()
    {
        if (Data == null)
        {
            Data = new();
        }
        
        if (Data.rankingScoreList.Count > 2)
        {
            if (Score.Value > Data.rankingScoreList.Min())
            {
                Data.rankingScoreList.Remove(Data.rankingScoreList.Min());
                Data.rankingScoreList.Add(Score.Value);
            }
        }
        else
        {
            Data.rankingScoreList.Add(Score.Value);
        }

        Data.rankingScoreList = Data.rankingScoreList.OrderByDescending(x => x).ToList();

        SaveLoader.Instance.data.flappyBirdData = Data;
        SaveLoader.Instance.Save();
    }
}

[System.Serializable]
public class FlappyBirdData : ISavable
{
    public List<int> rankingScoreList = new();
}