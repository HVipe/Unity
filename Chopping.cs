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
        // �������Ƿ񿿽������ֳָ�ͷ
        if (!timerActive && isHoldingAxe && Vector3.Distance(transform.position, tree.transform.position) <= 5f)
        {
            interactionText.text = "Time to unleash your excitement in 60s! Press R to start.";
            interactionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartTimer();
            }
        }

        // �����ʱ���߼�
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
        // ��������ƶ����ӽǣ�����ʵ�����������PlayerController�ű�
        // ʾ����GetComponent<PlayerController>().enabled = false;
    }

    void UnlockPlayerMovement()
    {
        // ��������ƶ����ӽ�
        // ʾ����GetComponent<PlayerController>().enabled = true;
    }
}
