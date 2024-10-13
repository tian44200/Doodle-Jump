using UnityEngine;
using UnityEngine.UI;  // 用于处理 UI 元素
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public Transform player;  // 
    public TextMeshProUGUI scoreText;    // UI Text 用于显示分数
        public TextMeshProUGUI highScoreGlobalText; // UI Text to display the highest score on end page


    private float highestPoint = 0f;  // 记录玩家到达的最高点

    private int score = 0;  // 分数

    void Start()
    {
        int highScoreGlobal = PlayerPrefs.GetInt("HighScoreGlobal", 0);
        scoreText.text = score.ToString();  // 初始化分数显示
        highScoreGlobalText.text = highScoreGlobal.ToString(); // Show the highest score on the end page (initially hidden
    }

    void Update()
    {
        // 检查玩家是否跳到了新的高度
        if (player.position.y > highestPoint)
        {
            highestPoint = player.position.y;  // 更新最高点
            score = Mathf.FloorToInt(highestPoint);  // 将高度转换为整数作为分数
            scoreText.text = score.ToString();  // 更新 UI 显示
        }
    }

    // Call this method when the player dies
    public void OnPlayerDeath()
    {
        // Load the saved high score
        int highScoreGlobal = PlayerPrefs.GetInt("highScoreGlobal", 0);

        // If the current score is higher than the saved high score, update it
        if (score > highScoreGlobal)
        {
            PlayerPrefs.SetInt("highScoreGlobal", score);
            PlayerPrefs.Save();  // Ensure the new high score is saved to disk
        }

        // Update the high score text for the end page
        highScoreGlobalText.text = PlayerPrefs.GetInt("highScoreGlobal").ToString();
    }
}
