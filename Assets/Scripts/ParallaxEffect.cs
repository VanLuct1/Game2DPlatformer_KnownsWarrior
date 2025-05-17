using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //
    Vector2 startingPosition; // Lưu trữ vị trí ban đầu của đối tượng (trục X và Y).
    // 
    float startingZ; // Lưu trữ vị trí ban đầu của đối tượng (trục Z).

    //Tính toán khoảng cách mà camera đã di chuyển từ vị trí ban đầu. Đây là vector 2D (X, Y).
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    //Tính khoảng cách giữa đối tượng hiện tại và đối tượng mà camera theo dõi (theo trục Z).
    //Điều này giúp xác định mức độ parallax.
    float distanceFromTarget => transform.position.z - followTarget.position.z;

    //Tính toán mặt phẳng cắt (clipping plane) của camera.
    //Nếu đối tượng nằm trước camera, sử dụng farClipPlane; nếu nằm sau, sử dụng nearClipPlane.
    float clippingPlane => (cam.transform.position.z + (distanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //Tính toán hệ số parallax. Hệ số này xác định mức độ di chuyển của đối tượng dựa trên khoảng cách của nó so với camera.
    float parallaxFactor => Mathf.Abs(distanceFromTarget) / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

   
    void Update()
    {
        //1.	camMoveSinceStart * parallaxFactor:
        //Tính toán vị trí mới của đối tượng dựa trên sự di chuyển của camera và hệ số parallax.
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
