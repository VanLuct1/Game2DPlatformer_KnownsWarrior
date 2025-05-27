using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player; // Transform của player
    public Camera minimapCamera; // Camera của minimap
    public Vector2 worldBoundsMin; // Giới hạn tối thiểu (x, y) của bản đồ
    public Vector2 worldBoundsMax; // Giới hạn tối đa (x, y) của bản đồ

    void LateUpdate()
    {
        // Kiểm tra null để tránh lỗi
        if (player == null || minimapCamera == null)
        {
            Debug.LogWarning("Player hoặc MinimapCamera chưa được gán!");
            return;
        }

        // Lấy vị trí của player
        float x = Mathf.Clamp(player.position.x, worldBoundsMin.x, worldBoundsMax.x);
        float y = Mathf.Clamp(player.position.y, worldBoundsMin.y, worldBoundsMax.y);

        // Cập nhật vị trí của minimap camera
        minimapCamera.transform.position = new Vector3(
            x,
            y,
            minimapCamera.transform.position.z // Giữ độ cao Z cố định
        );
    }
}