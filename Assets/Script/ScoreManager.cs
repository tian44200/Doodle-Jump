using UnityEngine;
using UnityEngine.UI;  // 用于处理 UI 元素
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public Transform player;  // Doodle 角色的 Transform，用来获取它的 Y 轴高度
    public TextMeshProUGUI scoreText;    // UI Text 用于显示分数

    private float highestPoint = 0f;  // 记录玩家到达的最高点
    private int score = 0;  // 分数

    void Start()
    {
        scoreText.text = score.ToString();  // 初始化分数显示
    }

    void Update()
    {
        Debug.Log("ScoreManager is running...");
        // 检查玩家是否跳到了新的高度
        if (player.position.y > highestPoint)
        {
            highestPoint = player.position.y;  // 更新最高点
            score = Mathf.FloorToInt(highestPoint);  // 将高度转换为整数作为分数
            scoreText.text = score.ToString();  // 更新 UI 显示
        }
    }
}
