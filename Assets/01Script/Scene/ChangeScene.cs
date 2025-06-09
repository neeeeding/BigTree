using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01Script.Scene
{
    public class ChangeScene : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] string sceneName;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                Button(sceneName);
            }
        }

        public void Button(string name)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitBtn()
        {
            Application.Quit();
        }

    }
}
