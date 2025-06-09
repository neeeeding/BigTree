using _01Script.Manager;
using _01Script.UI;
using UnityEngine;

namespace _01Script.Obj
{
    public class Key : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private KeyManager.CheckKeyType keyType; //열쇠 종류
        [Header("Need")]
        [SerializeField] private KeyManager key; //열쇠 얻기
        [SerializeField] private DialogManager dialogManager; //대화
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                dialogManager.DoDialog(new string[]{"열쇠를 얻었다."});
                key.GetKey(keyType);
                Destroy(gameObject);
            }
        }
    }
}