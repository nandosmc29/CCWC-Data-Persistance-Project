using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int currentPoints;
    public string playerName;
    private HighScore highscore = new HighScore();
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
        setPlayerName();
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
        currentPoints += point;
        ScoreText.text = $"Score : {currentPoints}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        updateHighScore();
    }

    public void setPlayerName()
    {
        playerName = 
            (ScenePersistentData.Instance != null && !string.IsNullOrEmpty(ScenePersistentData.Instance.playerName)) 
            ? playerName = ScenePersistentData.Instance.playerName 
            : playerName = "AAA";
    }

    [System.Serializable]
    class HighScore
    {
        public string playerName;
        public int points;
    }

    public void SaveHighScore()
    {
        string json = JsonUtility.ToJson(highscore);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            highscore = JsonUtility.FromJson<HighScore>(json);
            updateHighScoreText(highscore.playerName, highscore.points);
        }
    }

    private void updateHighScore()
    {
        Debug.Log(highscore.points);
        Debug.Log(currentPoints);
        if (currentPoints > highscore.points)
        {
            Debug.Log("higher");
            highscore.playerName = playerName;
            highscore.points = currentPoints;
            updateHighScoreText(highscore.playerName, highscore.points);
            SaveHighScore();
        }
    }

    private void updateHighScoreText(string playername, int points)
    {
        BestScoreText.text = $"Best Score : {playername} {points}";
    }
}
