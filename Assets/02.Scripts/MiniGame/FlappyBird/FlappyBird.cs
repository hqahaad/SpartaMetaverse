using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlappyBird : MiniGame, IMiniGame
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
    private bool isGameOver = false;

    public BindingData<int> score { get; } = new(1);

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

    private IEnumerator SpawnObstacleCo()
    {
        while (!isGameOver)
        {
            var obs = pool.FirstOrDefault(x => !x.gameObject.activeSelf);

            if (obs == null)
                yield break;

            obs.Spawn(Random.Range(minInterval, maxInterval));
            obs.transform.position = transform.position + new Vector3(0f, Random.Range(-maxRandomPosY, maxRandomPosY), 0f);

            yield return delayInstruction;

            AddScore();
        }
    }

    public void SetBlockSpeed(float blockSpeed)
    {
        this.blockSpeed = blockSpeed;
    }

    public string GetMiniGameName()
    {
        return "FlappyBird";
    }

    public void AddScore()
    {
        score.Value += 1;
    }

    public void SaveScore()
    {
        
    }
}
