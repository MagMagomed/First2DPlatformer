using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public Transform target; // ����, �� ������� ������� ������
    public float smoothing = 5f; // ����������� �������� ������
    public Vector2 offset = new Vector2(0f, 1f); // �������� ������ ������������ ����
    public float minCameraY = -100f; // ����������� �������� Y ��� ������� ������

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        // ����������� ������� ������ �� Y
        if (transform.position.y < minCameraY)
        {
            transform.position = new Vector3(transform.position.x, minCameraY, transform.position.z);
        }
    }
}
