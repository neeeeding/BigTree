using System;
using UnityEngine;

namespace _01Script.ObjUI
{
    public class ShowMap : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private GameObject map;

        private bool isMap;

        private void Awake()
        {
            isMap = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isMap = true;
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                isMap = false;
            }
            
            map.gameObject.SetActive(isMap);
        }
    }
}
