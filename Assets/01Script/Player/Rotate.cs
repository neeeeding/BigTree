using System;
using UnityEngine;

namespace _01Script.Player
{
    public class Rotate : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float _speed; //움직이는 속도
        [Header("Need")]
        [SerializeField] private GameObject cam; //카메라

        private Vector2 screneSize; //화면 크기
        private Vector2 mousePos; //마우스 위치

        private void Awake()
        {
            screneSize = Camera.main.pixelRect.size;
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                MousePos();   
            }
        }

        private void MousePos() // 마우스 위치에 따라 회전
        {
            mousePos.x += Input.GetAxis("Mouse X") * _speed;
            mousePos.y += Input.GetAxis("Mouse Y") * _speed;
            
            
            mousePos.y = Mathf.Clamp(mousePos.y, -30f, 80f);

            gameObject.transform.localEulerAngles = new Vector3(0, mousePos.x, 0);
            cam.transform.localEulerAngles = new Vector3(-mousePos.y,0, 0);
        }
    }
}
