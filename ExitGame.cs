using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // 引入TextMeshPro命名空间

public class ExitGame : MonoBehaviour
{
    public GameObject plane;  // Plane对象
    public TextMeshProUGUI infoText;  // TextMeshPro文本组件的引用
    public float triggerDistance = 5.0f;  // 触发距离

    private void Start()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false);  // 初始时隐藏文本
    }

    void Update()
    {
        if (plane != null)
        {
            // 计算与Plane对象的距离
            float distance = Vector3.Distance(transform.position, plane.transform.position);

            // 显示或隐藏文本提示
            if (distance <= triggerDistance)
            {
                if (infoText != null)
                    infoText.gameObject.SetActive(true);  // 显示文本
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // 退出游戏
                    Application.Quit();

                    // 如果是Unity编辑器中，停止播放
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
            }
            else
            {
                if (infoText != null)
                    infoText.gameObject.SetActive(false);  // 隐藏文本
            }
        }
    }
}
