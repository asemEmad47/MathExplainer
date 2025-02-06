using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BigPuzzleGameManager : MonoBehaviour
{
    [SerializeField] GameObject bigPrefab; //It's a button

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] bool[] spawnPointsUsed;

    [SerializeField] GameObject rightAnsUI;
    [SerializeField] GameObject wrongAnsUI;

    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI ScoreTxt;
    [SerializeField] RawImage Emoji;
    [SerializeField] GameObject EndGamePanel;
    public int[] numbers; //numbers in the scene

    public int currentNum; //smallest number

    public int index;

    int score = 0;

    public static BigPuzzleGameManager instance;

    public int difficulty = 1;

    private bool IsEnded = false;


    Timer timer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timer = GetComponent<Timer>();
        SpawnBigPuzzle();
    }

    public void SpawnBigPuzzle()
    {
        ResetspawnPoints();
        numbers = GenerateNumbers();
        Debug.Log($"{numbers[0]} {numbers[1]} {numbers[2]} {numbers[3]}");
        for (int i = 0; i < spawnPoints.Length && i < difficulty + 1 && !IsEnded; i++)
        {
            Vector3 prefabspawnPoints = RandomSpawnPoints() + new Vector3(UnityEngine.Random.Range(-150, 150), 0, 0);
            Instantiate(bigPrefab, prefabspawnPoints, Quaternion.identity, spawnPoints[i]);
        }
        Array.Sort(numbers);
        currentNum = numbers[0];
    }

    void ResetspawnPoints()
    {
        for (int i = 0; i < spawnPointsUsed.Length; i++)
        {
            spawnPointsUsed[i] = false;
        }
    }

    Vector3 RandomSpawnPoints()
    {

        // Iterate until a valid spawn point is found
        int randomIndex;
        do
        {
            // Get a random index from the array
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        } while (spawnPointsUsed[randomIndex]); // Repeat if the spawn point is already used

        // Mark the spawn point as used
        spawnPointsUsed[randomIndex] = true;

        // Get the random transform from the array
        return spawnPoints[randomIndex].position;

    }

    private void Update()
    {
        if (timer.remainingDuration == 40)
        {
            difficulty = 2;
        }
        else if (timer.remainingDuration == 20)
        {
            difficulty = 3;
        }
        else if(timer.isDone&& !IsEnded)
        {
            EndGame();
        }
    }

    public void UpdateCurrentNum()
    {
        if (index < ButtonGameLogic.index - 1)
        {
            index++;
            currentNum = numbers[index];
        }
        else
        {
            Right();
        }
    }

    public void Right() { StartCoroutine("RightAnswer"); }
    IEnumerator RightAnswer()
    {
        ButtonGameLogic.index = 0;
        score += 10;
        UpdateScore(score);
        rightAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        rightAnsUI.SetActive(false);
        SpawnBigPuzzle();
    }

    public void Wrong() { StartCoroutine("WrongAnswer"); }
    IEnumerator WrongAnswer()
    {
        wrongAnsUI.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        wrongAnsUI.SetActive(false);
        SpawnBigPuzzle();
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    int[] GenerateNumbers()
    {
        index = 0;
        int[] numbers = { 0, 0, 0, 0 };
        int randomNum;
        for (int i = 0; i < numbers.Length; i++)
        {
            while (true)
            {
                randomNum = UnityEngine.Random.Range(-50, 100);
                if (!numbers.Contains(randomNum))
                {
                    numbers[i] = randomNum;
                    break;
                }
            }

        }
        return numbers;
    }
    private void EndGame()
    {
        IsEnded = true;
        EndGamePanel.SetActive(true);

        EndGameScript endGameScript = gameObject.AddComponent<EndGameScript>();
        endGameScript.IntilizeComponents(score, ScoreTxt, Emoji);
        endGameScript.EndGame();
    }
}