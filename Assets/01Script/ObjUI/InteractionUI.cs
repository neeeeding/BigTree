using TMPro;
using UnityEngine;

namespace _01Script.ObjUI
{
    public class InteractionUI : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private string keyName; //키 이름
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI showText; // 보여줄 텍스트

        private void Awake()
        {
            showText.text = $"{keyName}로\n상호작용";
            showText.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                showText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                showText.gameObject.SetActive(false);
            }
        }
    }
}
