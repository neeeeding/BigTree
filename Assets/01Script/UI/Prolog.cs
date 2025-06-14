using System;
using _01Script.Manager;
using UnityEngine;

namespace _01Script.UI
{
    public class Prolog : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private DialogManager dialogManager; //대화
        [SerializeField] private GameObject turorial;

        private void Awake()
        {
            turorial.SetActive(false);
        }

        private void Start()
        {
            dialogManager.DoDialog(new string[]{ "...예전의 모습과 달리\n정말 황폐하구나", "그래도 기억을 더듬으면 집으로\n돌아갈 수 있을거야."});
        }

        private void Update()
        {
            if (!dialogManager.CanDialog())
            {
                turorial.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}