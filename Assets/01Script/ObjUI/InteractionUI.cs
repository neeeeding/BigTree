using System;
using System.Collections.Generic;
using _01Script.Manager;
using _01Script.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.ObjUI
{
    public class InteractionUI : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField]private string[] _words; // 실행할 대화
        [SerializeField]private MeshRenderer selectMesh; //매쉬 바꾸려면
        
        [CanBeNull] private static InteractionUI _runningMe; //실행중인
        private static TextMeshProUGUI _showText; // 보여줄 텍스트
        private static DialogManager _dialogManager; //대화
        private static Material _outLine; //잡고 있음을 보여주려고
        private static Camera _camera;

        private bool isOutLine; // 아웃 라인 있는지
        
        private void Start()
        {
            isOutLine =  true;
            if (_showText != null)
            {
                _showText.text = $"F로\n상호작용";
                _showText.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            MousePos();
        }

        public void ChangeWords(string[] words)
        {
            _words = words;
        }

        public bool IsYou(GameObject you, bool isChat = false) //지금 보여주고 있는게 본인이 맞다
        {
            if (!_runningMe)
            {
                return false;
            }

            if (isChat)
            {
                return _dialogManager.CanDialog();
            }
            return _runningMe.gameObject ==  you && _dialogManager.CanDialog();
        }

        private void ShowText() //보여주기
        {
            if (!_dialogManager.CanDialog())
            {
                _showText.gameObject.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.F) && _words != null)
            {
                _dialogManager.DoDialog(_words);
                _showText.gameObject.SetActive(false);
            }
        }
        
        private void MousePos() //마우스 위치 찾기
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 20; // 광선 최대 거리

            bool everyNot = true; //가지고 있는애가 없음

            if (Physics.Raycast(ray, out hit, maxDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent(out InteractionUI me)) //해당 스크립트를 가지고 있음
                {
                    if (me.isOutLine&& me.selectMesh)
                    {
                        List<Material> materialList = new List<Material>(me.selectMesh.materials);
                        materialList.Add(_outLine);
                        me.selectMesh.materials = materialList.ToArray();
                        me.isOutLine = false;
                    }
                    
                    _runningMe = me;
                    me.ShowText();
                    everyNot = false;
                }
            }
            
            if (everyNot)
            {
                if (selectMesh && !isOutLine)
                {
                    List<Material> materialList = new List<Material>(selectMesh.materials);
                    for (int i = materialList.Count - 1; i >= 0; i--)
                    {
                        if (materialList[i].name == _outLine.name || 
                            materialList[i].name == _outLine.name + " (Instance)")
                        {
                            materialList.RemoveAt(i);
                            break; // 첫 번째 것만 제거
                        }
                    }
                    selectMesh.materials = materialList.ToArray();
                }
                _runningMe = null;
                isOutLine =  true;
                _showText.gameObject.SetActive(false);
            }
        }

        public static void Setting(TextMeshProUGUI text, DialogManager dialog, Material material) //텍스트 정하기
        {
            _showText = text;
            _dialogManager = dialog;
            _outLine = material;
            _camera = Camera.main;
        }
    }
}
