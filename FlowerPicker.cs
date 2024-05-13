using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowerPicker : MonoBehaviour
{
    public GameObject table;
    public GameObject[] flowers;
    public float interactionDistance = 2f;
    public float tableInteractionDistance = 2f;
    public Transform player;
    public TextMeshProUGUI flowerCountText;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI tableText;
    public FirstPersonMovement playerMovementScript;

    private int flowerCount = 0;
    private float speedBoostDuration = 10f; // Speed boost duration

    void Start()
    {
        flowerCountText.gameObject.SetActive(false);
        pickupText.gameObject.SetActive(false);
        tableText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckFlowerInteraction();
        CheckTableInteraction();
    }

    void CheckFlowerInteraction()
    {
        bool flowerInRange = false;
        foreach (GameObject flower in flowers)
        {
            if (flower.activeSelf && Vector3.Distance(player.position, flower.transform.position) <= interactionDistance)
            {
                flowerInRange = true;
                break;
            }
        }
        pickupText.gameObject.SetActive(flowerInRange);
        if (Input.GetKeyDown(KeyCode.F) && flowerInRange)
        {
            PickNearestFlower();
        }
    }

    void CheckTableInteraction()
    {
        bool tableInRange = Vector3.Distance(player.position, table.transform.position) <= tableInteractionDistance;
        tableText.gameObject.SetActive(tableInRange);
        tableText.text = "Press R to drop 10 flowers and make speed potion!";

        if (tableInRange && Input.GetKeyDown(KeyCode.R) && flowerCount >= 10)
        {
            flowerCount -= 10;
            flowerCountText.text = "Flowers: " + flowerCount;
            flowerCountText.gameObject.SetActive(flowerCount > 0);
            playerMovementScript.ActivateSpeedBoost(speedBoostDuration);
        }
    }

    void PickNearestFlower()
    {
        GameObject nearestFlower = null;
        float minDistance = float.MaxValue;

        foreach (GameObject flower in flowers)
        {
            if (flower.activeSelf)
            {
                float distanceToFlower = Vector3.Distance(player.position, flower.transform.position);
                if (distanceToFlower < minDistance)
                {
                    minDistance = distanceToFlower;
                    nearestFlower = flower;
                }
            }
        }

        if (nearestFlower != null && minDistance <= interactionDistance)
        {
            nearestFlower.SetActive(false);
            flowerCount++;
            flowerCountText.text = "Flowers: " + flowerCount;
            flowerCountText.gameObject.SetActive(true);
        }
    }
}
