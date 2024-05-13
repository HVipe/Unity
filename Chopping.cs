using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerAndCountInteraction : MonoBehaviour
{
    public GameObject tree;
    public GameObject axe;
    public Transform handPosition;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI countdownText;

    private bool isHoldingAxe = false;
    private bool timerActive = false;
    private float countdown = 60f;
    private int pressCount = 0;

    void Update()
    {
        // 检查玩家是否靠近树且手持斧头
        if (!timerActive && isHoldingAxe && Vector3.Distance(transform.position, tree.transform.position) <= 5f)
        {
            interactionText.text = "Time to unleash your excitement in 60s! Press R to start.";
            interactionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartTimer();
            }
        }

        // 处理计时器逻辑
        if (timerActive)
        {
            countdown -= Time.deltaTime;
            countdownText.text = "Time left: " + Mathf.CeilToInt(countdown).ToString();

            if (Input.GetKeyDown(KeyCode.R))
            {
                pressCount++;
            }

            if (countdown <= 0)
            {
                EndTimer();
            }
        }
    }

    void StartTimer()
    {
        timerActive = true;
        countdown = 60f;
        pressCount = 0;
        interactionText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        LockPlayerMovement();
    }

    void EndTimer()
    {
        timerActive = false;
        countdownText.gameObject.SetActive(false);
        interactionText.text = $"You got {pressCount} pieces of wood! Have you relaxed?";
        interactionText.gameObject.SetActive(true);
        UnlockPlayerMovement();
    }

    void LockPlayerMovement()
    {
        // 锁定玩家移动和视角，具体实现依赖于你的PlayerController脚本
        // 示例：GetComponent<PlayerController>().enabled = false;
    }

    void UnlockPlayerMovement()
    {
        // 解锁玩家移动和视角
        // 示例：GetComponent<PlayerController>().enabled = true;
    }
}
