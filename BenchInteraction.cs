using System.Collections;
using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间

public class BenchInteraction : MonoBehaviour
{
    public Camera playerCamera; // 玩家的第一人称摄像机
    public Camera viewpointCamera; // 休息时使用的摄像机
    public TextMeshProUGUI interactionText; // 使用TextMeshProUGUI显示交互提示
    public GameObject playerObject; // 代表玩家的capsule对象
    public AudioSource sceneBGM; // 场景的背景音乐源
    public AudioSource benchBGM; // benchCamera 激活时的背景音乐源
    public float interactionDistance = 3f; // 玩家与Bench的交互距离

    private bool isResting = false; // 玩家是否正在休息

    void Update()
    {
        if (isResting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // 离开休息状态
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
                        // 进入休息状态
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

        // 锁定玩家移动
        TogglePlayerMovement(false);

        // 启用benchCamera的视角控制
        BenchCameraControl.EnableRotation(true);

        // 隐藏代表玩家的capsule object
        if (playerObject != null)
            playerObject.SetActive(false);

        // 暂停场景BGM并播放Bench BGM
        if (sceneBGM != null)
            sceneBGM.Pause(); // 暂停当前场景的音乐
        if (benchBGM != null)
            benchBGM.Play(); // 播放 bench 视角的音乐
    }

    void LeaveRestingState()
    {
        isResting = false;
        playerCamera.enabled = true;
        viewpointCamera.enabled = false;
        interactionText.text = "";

        // 解锁玩家移动
        TogglePlayerMovement(true);

        // 禁用benchCamera的视角控制
        BenchCameraControl.EnableRotation(false);

        // 显示代表玩家的capsule object
        if (playerObject != null)
            playerObject.SetActive(true);

        // 恢复场景BGM并停止Bench BGM
        if (sceneBGM != null)
            sceneBGM.Play(); // 恢复播放场景的音乐
        if (benchBGM != null)
            benchBGM.Stop(); // 停止播放 bench 视角的音乐
    }

    void TogglePlayerMovement(bool canMove)
    {
        // 假设有一个FirstPersonMovement脚本控制玩家移动
        var movementScript = playerCamera.transform.parent.GetComponent<FirstPersonMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = canMove;
        }
    }
}
