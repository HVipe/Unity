using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractWithTomatoes : MonoBehaviour
{
    public Camera playerCamera; // �������ͷ����
    public float interactionDistance = 2f; // ��tomato������������
    public TextMeshProUGUI pickupText; // ʹ��TextMeshProUGUI���͵ı�����ʾ��ʾ��Ϣ
    public LayerMask tomatoLayerMask; // ���ڹ������߼���LayerMask��ר������Tomato Layer

    private GameObject currentTomato = null; // ��ǰ�����ǰ��tomato����

    void Update()
    {
        // �����ǰ����tomato��������ʾ��ֱ�ӷ��أ������ظ����
        if (currentTomato != null)
        {
            pickupText.gameObject.SetActive(false);
            return;
        }

        // ���߼���Բ��������ǰ��tomato

        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * interactionDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, tomatoLayerMask))
        {
            // ������߻���������Tomato Layer�Ķ���
            ShowPickupText(hit.collider.gameObject);

            if (Input.GetKeyDown(KeyCode.F))
            {
                // ֱ������tomato����
                currentTomato = hit.collider.gameObject;
                currentTomato.SetActive(false);
                pickupText.gameObject.SetActive(false);
                currentTomato = null; // ������ã����������µ�tomato����
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

