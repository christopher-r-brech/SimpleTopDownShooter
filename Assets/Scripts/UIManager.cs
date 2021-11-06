using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;

    [SerializeField] private TMPro.TextMeshProUGUI healthTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI scoreTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI highScoreTextMesh;

    [SerializeField] private int startHealth = 100;
    [SerializeField] private int startScore = 0;

    private string highScoreText;
    private string healthText;
    private string scoreText;

    private int highScore;
    private int health;
    private int score;

    private void Awake()
    {
        if (_GameManager == null) _GameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Start()
    {
        highScore = 0;
        health = startHealth;
        score = startScore;

        highScoreText = highScoreTextMesh.text;
        healthText = healthTextMesh.text;
        scoreText = scoreTextMesh.text;

        highScoreTextMesh.text = highScoreText + highScore;
        healthTextMesh.text = healthText + health;
        scoreTextMesh.text = scoreText + score;
    }
   
    public void IncreaseScore()
    {
        score += 100;
        scoreTextMesh.text = scoreText + score;
    }

    public void DecreaseHealth(bool largeHit = false)
    {
        health -= largeHit ? 20 : 10;
        if (health <= 0)
        {
            ResetGameProgress();
            return;
        }
        healthTextMesh.text = healthText + health;
    }

    private void ResetGameProgress()
    {
        if (score > highScore)
        {
            highScore = score;
        }
        health = startHealth;
        score = startScore;

        highScoreTextMesh.text = highScoreText + highScore;
        healthTextMesh.text = healthText + health;
        scoreTextMesh.text = scoreText + score;
        _GameManager.OnGameReset();
    }
}
