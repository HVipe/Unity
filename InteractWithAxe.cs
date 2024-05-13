using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractWithAxe : MonoBehaviour
{
    public GameObject axe;
    public Transform handPosition;
    public Camera playerCamera;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI swingText;
    public TextMeshProUGUI challengeText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI woodCountText;
    public TextMeshProUGUI successText;
    public Animator axeAnimator;
    public float interactionDistance = 2f;
    public float dropForce = 5f;
    public GameObject[] targetObjects; // 使用数组来处理多个目标对象
    public BenchInteraction benchInteractionScript;
    public FlowerPicker flowerPickerScript;

    private bool isHoldingAxe = false;
    private bool challengeActive = false;
    private float challengeStartedTime;
    private float challengeDuration = 30f;
    private float lastRPressTime;
    private int woodCount;

    void Start()
    {
        axe.GetComponent<Rigidbody>().isKinematic = true;
        HideText();
        timerText.gameObject.SetActive(false);
        woodCountText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
        lastRPressTime = Time.time;
    }

    void Update()
    {
        float distanceToAxe = Vector3.Distance(axe.transform.position, transform.position);

        if (isHoldingAxe)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DropAxe();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SwingAxe();
                // 改变逻辑以确认是否触发挑战模式
                if (!challengeActive)
                {
                    if (IsAnyTargetWithinRange())
                    {
                        StartChallenge();
                    }
                }
                else
                {
                    CollectWood(); // 只有在挑战模式激活的情况下收集木头
                }
            }

            pickupText.gameObject.SetActive(false);
        }
        else
        {
            HandleNotHoldingAxe(distanceToAxe);
        }

        if (challengeActive)
        {
            UpdateTimer();
            woodCountText.text = $"Wood Collected: {woodCount}";
        }
    }

    bool IsAnyTargetWithinRange()
    {
        foreach (var target in targetObjects)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= interactionDistance)
            {
                return true;
            }
        }
        return false;
    }

    void HandleNotHoldingAxe(float distanceToAxe)
    {
        if (distanceToAxe <= interactionDistance)
        {
            pickupText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUpAxe();
            }
        }
        else
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    void StartChallenge()
    {
        challengeActive = true;
        challengeStartedTime = Time.time;
        challengeText.text = "Try to collect 100 pieces of wood in 30s!";
        challengeText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        woodCountText.gameObject.SetActive(true);
        woodCount = 0;

        pickupText.gameObject.SetActive(false);
        swingText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
    }

    void UpdateTimer()
    {
        float timeLeft = challengeDuration - (Time.time - challengeStartedTime);
        if (timeLeft > 0)
        {
            timerText.text = $"Time Left: {timeLeft:F1}s";
        }
        else
        {
            EndChallenge();
        }
    }

    void EndChallenge()
    {
        challengeActive = false;
        timerText.gameObject.SetActive(false);
        woodCountText.gameObject.SetActive(false);
        challengeText.gameObject.SetActive(false);

        if (isHoldingAxe)
        {
            swingText.gameObject.SetActive(true);
        }

        if (woodCount >= 100)
        {
            successText.text = "Good for you! Feel better now?";
            successText.gameObject.SetActive(true);
        }
    }

    void HideText()
    {
        pickupText.gameObject.SetActive(false);
        swingText.gameObject.SetActive(false);
        challengeText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        woodCountText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
    }

    void PickUpAxe()
    {
        isHoldingAxe = true;
        axe.transform.SetParent(handPosition);
        axe.transform.localPosition = Vector3.zero;
        axe.transform.localRotation = Quaternion.identity;
        axe.GetComponent<Rigidbody>().isKinematic = true;
        axe.GetComponent<Collider>().enabled = false;
        axeAnimator.enabled = true;
        axe.SetActive(true);
        swingText.gameObject.SetActive(true);

        // Disable other interactions
        if (benchInteractionScript != null)
            benchInteractionScript.enabled = false;
        if (flowerPickerScript != null)
            flowerPickerScript.enabled = false;
    }

    void DropAxe()
    {
        isHoldingAxe = false;
        axe.transform.SetParent(null);
        Rigidbody axeRb = axe.GetComponent<Rigidbody>();
        axeRb.isKinematic = false;
        axe.GetComponent<Collider>().enabled = true;
        Vector3 dropPosition = transform.position + transform.forward * 1f;
        axe.transform.position = dropPosition;
        axeRb.AddForce(transform.forward * dropForce, ForceMode.Impulse);
        axeAnimator.enabled = false;
        axe.SetActive(true);
        HideText();

        // Enable other interactions
        if (benchInteractionScript != null)
            benchInteractionScript.enabled = true;
        if (flowerPickerScript != null)
            flowerPickerScript.enabled = true;

        if (challengeActive)
        {
            EndChallenge();
        }
    }


    void SwingAxe()
    {
        float timeSinceLastPress = Time.time - lastRPressTime;
        float speed = Mathf.Clamp(1f / timeSinceLastPress, 0.5f, 3f);
        axeAnimator.speed = speed;
        axeAnimator.SetTrigger("SwingTrigger");
        lastRPressTime = Time.time;
    }

    public void CollectWood()
    {
        woodCount++;
    }
}

