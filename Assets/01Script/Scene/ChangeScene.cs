using System;
using _01Script.ObjUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01Script.Scene
{
    public class ChangeScene : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] string sceneName;
        [Header("Need")]
        [SerializeField] private InteractionUI fUi; //상호작용 UI

        private bool isDialog; // 대화 끝났는지

        private void Awake()
        {
            isDialog = false;
        }

        private void Update()
        {
            if (fUi)
            {
                if (fUi.IsYou(gameObject))
                {
                    isDialog = true;
                }

                if (isDialog && !fUi.IsYou(gameObject)) 
                {
                    Button(sceneName);
                
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

    }
}
