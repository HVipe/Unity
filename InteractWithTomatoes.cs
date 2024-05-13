using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractWithTomatoes : MonoBehaviour
{
    public Camera playerCamera; // 玩家摄像头引用
    public float interactionDistance = 2f; // 与tomato互动的最大距离
    public TextMeshProUGUI pickupText; // 使用TextMeshProUGUI类型的变量显示提示信息
    public LayerMask tomatoLayerMask; // 用于过滤射线检测的LayerMask，专门用于Tomato Layer

    private GameObject currentTomato = null; // 当前玩家面前的tomato对象

    void Update()
    {
        // 如果当前持有tomato，隐藏提示并直接返回，避免重复检测
        if (currentTomato != null)
        {
            pickupText.gameObject.SetActive(false);
            return;
        }

        // 射线检测以查找玩家面前的tomato

        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * interactionDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, tomatoLayerMask))
        {
            // 如果射线击中了属于Tomato Layer的对象
            ShowPickupText(hit.collider.gameObject);

            if (Input.GetKeyDown(KeyCode.F))
            {
                // 直接隐藏tomato对象
                currentTomato = hit.collider.gameObject;
                currentTomato.SetActive(false);
                pickupText.gameObject.SetActive(false);
                currentTomato = null; // 清除引用，以允许检测新的tomato对象
            }
        }
        else
        {
            pickupText.gameObject.SetActive(false);
        }
    }

    void ShowPickupText(GameObject tomatoObject)
    {
        pickupText.gameObject.SetActive(true);
        pickupText.text = "Press F to pick up ";
    }

}

