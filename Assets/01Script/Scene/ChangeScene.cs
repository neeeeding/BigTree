using System;
using _01Script.Manager;
using _01Script.ObjUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01Script.Scene
{
    public class ChangeScene : MonoBehaviour,DialogDoScript
    {
        [Header("Setting")]
        [SerializeField] string sceneName;
        [Header("Need")]
        [SerializeField] private InteractionUI fUi; //상호작용 UI

        private bool isDialog; // 대화 끝났는지

        private void Awake()
        {
            isDialog = false;
            if (fUi)
            {
                fUi.SetDoScript(this);
            }
        }

        private void Update()
        {
            if (fUi)
            {
                if (!isDialog&&fUi.IsYou(gameObject))
                {
                    isDialog = true;
                }
            }
        }

        public void Button(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void QuitBtn()
        {
            Application.Quit();
        }

        public void Do()
        {
            Button(sceneName);
        }
    }
}
