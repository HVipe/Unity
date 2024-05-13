using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f; // ���һ����������¼y����ת
    public static bool canRotate = false;

    void Start()
    {
        // ��ʼ��y����תΪ��ǰ��y����ת
        yRotation = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // �����µ�x���y����ת
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 0f); // ����������ת�Է�ֹ��ת

            yRotation += mouseX; // �ۼ�y����ת

            // Ӧ���µ���ת
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    public static void EnableRotation(bool enable)
    {
        canRotate = enable;
    }
}
