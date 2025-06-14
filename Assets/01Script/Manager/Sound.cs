using System;
using UnityEngine;
using UnityEngine.UI;

namespace _01Script.Manager
{
    public class Sound : MonoBehaviour
    {
        [Header("Nedd")]
        [SerializeField] private Slider sound; //조절
        [SerializeField] private AudioSource clip; //음악
        [SerializeField] private GameObject setting; //세팅 창
        private void Awake()
        {
            sound.value = PlayerPrefs.GetFloat("sound");
            Close();
        }

        public void Setting() //세팅창 켜기
        {
            setting.SetActive(true);
            Time.timeScale = 0;
        }

        public void Close() //세팅창 닫기
        {
            setting.SetActive(false);
            Time.timeScale = 1;
        }

        public void SoundControl() //소리 조절
        {
            clip.volume = sound.value;
            PlayerPrefs.SetFloat("sound", sound.value);
        }
    }
}
