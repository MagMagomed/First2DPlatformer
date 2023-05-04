using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public Transform target; // Цель, за которой следует камера
    public float smoothing = 5f; // Сглаживание движения камеры
    public Vector2 offset = new Vector2(0f, 1f); // Смещение камеры относительно цели
    public float minCameraY = -100f; // Минимальное значение Y для позиции камеры

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        // Ограничение позиции камеры по Y
        if (transform.position.y < minCameraY)
        {
            transform.position = new Vector3(transform.position.x, minCameraY, transform.position.z);
        }
    }
}
