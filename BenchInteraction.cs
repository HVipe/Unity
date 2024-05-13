using System.Collections;
using UnityEngine;
using TMPro; // ����TextMeshPro�����ռ�

public class BenchInteraction : MonoBehaviour
{
    public Camera playerCamera; // ��ҵĵ�һ�˳������
    public Camera viewpointCamera; // ��Ϣʱʹ�õ������
    public TextMeshProUGUI interactionText; // ʹ��TextMeshProUGUI��ʾ������ʾ
    public GameObject playerObject; // ������ҵ�capsule����
    public AudioSource sceneBGM; // �����ı�������Դ
    public AudioSource benchBGM; // benchCamera ����ʱ�ı�������Դ
    public float interactionDistance = 3f; // �����Bench�Ľ�������

    private bool isResting = false; // ����Ƿ�������Ϣ

    void Update()
    {
        if (isResting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // �뿪��Ϣ״̬
                LeaveRestingState();
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                if (hit.collider.gameObject.name == "Bench")
                {
                    interactionText.text = "Press F to take a rest";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // ������Ϣ״̬
                        EnterRestingState();
                    }
                }
                else
                {
                    interactionText.text = "";
                }
            }
            else
            {
                interactionText.text = "";
            }
        }
    }

    void EnterRestingState()
    {
        isResting = true;
        playerCamera.enabled = false;
        viewpointCamera.enabled = true;
        interactionText.text = "Press F to leave";

        // ��������ƶ�
        TogglePlayerMovement(false);

        // ����benchCamera���ӽǿ���
        BenchCameraControl.EnableRotation(true);

        // ���ش�����ҵ�capsule object
        if (playerObject != null)
            playerObject.SetActive(false);

        // ��ͣ����BGM������Bench BGM
        if (sceneBGM != null)
            sceneBGM.Pause(); // ��ͣ��ǰ����������
        if (benchBGM != null)
            benchBGM.Play(); // ���� bench �ӽǵ�����
    }

    void LeaveRestingState()
    {
        isResting = false;
        playerCamera.enabled = true;
        viewpointCamera.enabled = false;
        interactionText.text = "";

        // ��������ƶ�
        TogglePlayerMovement(true);

        // ����benchCamera���ӽǿ���
        BenchCameraControl.EnableRotation(false);

        // ��ʾ������ҵ�capsule object
        if (playerObject != null)
            playerObject.SetActive(true);

        // �ָ�����BGM��ֹͣBench BGM
        if (sceneBGM != null)
            sceneBGM.Play(); // �ָ����ų���������
        if (benchBGM != null)
            benchBGM.Stop(); // ֹͣ���� bench �ӽǵ�����
    }

    void TogglePlayerMovement(bool canMove)
    {
        // ������һ��FirstPersonMovement�ű���������ƶ�
        var movementScript = playerCamera.transform.parent.GetComponent<FirstPersonMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = canMove;
        }
    }
}
