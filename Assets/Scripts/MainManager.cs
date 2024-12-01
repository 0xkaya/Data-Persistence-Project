using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText; // Display current score
    public Text HighestScoreText; // Display highest score with nickname
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {

        // Display the highest score with the player's nickname
        if (GameManager.Instance != null && HighestScoreText !=null)
        {
            HighestScoreText.text = $"Best Score: {GameManager.Instance.highestScorer} | {GameManager.Instance.highestScore}";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Update highest score if the current score exceeds it
        if (GameManager.Instance != null && m_Points > GameManager.Instance.highestScore)
        {
            GameManager.Instance.highestScore = m_Points;
            GameManager.Instance.highestScorer = GameManager.Instance.nickname;
            GameManager.Instance.SaveHighestScore(); // Save the new highest score

            // Update the displayed highest score
            HighestScoreText.text = $"Best Score: {GameManager.Instance.nickname} | {GameManager.Instance.highestScore}";
        }
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
