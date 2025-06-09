using _01Script.Manager;
using _01Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Obj
{
    public class Key : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private KeyManager.CheckKeyType keyType; //열쇠 종류
        [Header("Need")]
        [SerializeField] private KeyManager key; //열쇠 얻기
        [FormerlySerializedAs("dialog")] [SerializeField] private DialogManager dialogManager; //대화
        
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