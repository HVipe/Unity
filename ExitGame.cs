using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // ����TextMeshPro�����ռ�

public class ExitGame : MonoBehaviour
{
    public GameObject plane;  // Plane����
    public TextMeshProUGUI infoText;  // TextMeshPro�ı����������
    public float triggerDistance = 5.0f;  // ��������

    private void Start()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false);  // ��ʼʱ�����ı�
    }

    void Update()
    {
        if (plane != null)
        {
            // ������Plane����ľ���
            float distance = Vector3.Distance(transform.position, plane.transform.position);

            // ��ʾ�������ı���ʾ
            if (distance <= triggerDistance)
            {
                if (infoText != null)
                    infoText.gameObject.SetActive(true);  // ��ʾ�ı�
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // �˳���Ϸ
                    Application.Quit();

                    // �����Unity�༭���У�ֹͣ����
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
            }
            else
            {
                if (infoText != null)
                    infoText.gameObject.SetActive(false);  // �����ı�
            }
        }
    }
}
