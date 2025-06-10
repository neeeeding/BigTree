using System;
using _01Script.ObjUI;
using _01Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Manager
{
    public class InteractionUIManager : MonoBehaviour
    {
        
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private DialogManager  dialogManager;
        [SerializeField] private Material outLine;

        private void Awake()
        {
            InteractionUI.Setting(text,dialogManager,outLine);
        }
    }
}