using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f; // 添加一个变量来记录y轴旋转
    public static bool canRotate = false;

    void Start()
    {
        // 初始化y轴旋转为当前的y轴旋转
        yRotation = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 计算新的x轴和y轴旋转
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 0f); // 限制上下旋转以防止翻转

            yRotation += mouseX; // 累加y轴旋转

            // 应用新的旋转
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    public static void EnableRotation(bool enable)
    {
        canRotate = enable;
    }
}
