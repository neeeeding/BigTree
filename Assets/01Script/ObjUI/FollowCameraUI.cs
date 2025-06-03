using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _01Script.ObjUI
{
    public class FollowCameraUI : MonoBehaviour
    {
        [SerializeField] private Transform text;

        private void Update()
        {
            SetButtonUILookAtCamera();
        }
        
        private void SetButtonUILookAtCamera()
        {
            text.LookAt(Camera.main.transform);
        }
    }
}
