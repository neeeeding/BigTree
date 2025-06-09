using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.UI
{
    public class DialogManager : MonoBehaviour
    {
        [FormerlySerializedAs("isPlay")]
        [Header("Setting")]
        [SerializeField] private float delay = 0.1f; // 출력 딜레이
        [Header("Show")]
        [SerializeField]private string[] words = { "...예전의 모습과 달리\n정말 황폐하구나", "그래도 기억을 더듬으면 집으로\n돌아갈 수 있을거야."};
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI text; // 말
        [SerializeField] private GameObject im; // 가림막
        
        private int word; //현재 출력하고 있는거
        private int num; //글자 순서

        private void Awake()
        {
            im.SetActive(false);
            text.gameObject.SetActive(false);
        }

        public void Skip() //바로 넘기기
        { 
            if (num >= words[word].Length - 1)
            {
                word++;
            }
            else
            {
                num = words[word].Length -1;
                text.text = words[word];
                return;
            }
             
            if (word >= words.Length)
            {
                im.SetActive(false);
                text.gameObject.SetActive(false);
                return;
            }
            Next();
        }

        private void Next()
        {
            StartCoroutine(TextPrint());
        }
        
        public void DoDialog(string[] w) //대화 실행
        {
            words = w;
            word = 0;
            num = 0;
            Next();
        }

        private IEnumerator TextPrint() //한 글자씩 출력
        {
            im.SetActive(true);
            text.gameObject.SetActive(true);
            
            text.text = "";
            
            for(num  = 0; num < words[word].Length; num++)
            {
                text.text += words[word][num];
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
