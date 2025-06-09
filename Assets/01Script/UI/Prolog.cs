using System;
using _01Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.UI
{
    public class Prolog : MonoBehaviour
    {
        [Header("Need")]
        [FormerlySerializedAs("dialog")] [SerializeField] private DialogManager dialogManager; //대화

        private void Start()
        {
            dialogManager.DoDialog(new string[]{ "...예전의 모습과 달리\n정말 황폐하구나", "그래도 기억을 더듬으면 집으로\n돌아갈 수 있을거야."});
        }
    }
}