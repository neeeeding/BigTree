using System;
using _01Script.Manager;
using _01Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Obj
{
    public class Door : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private KeyManager.CheckKeyType keyType; //열쇠 종류
        [SerializeField] private GameObject nextDoor;
        [Header("Need")]
        [SerializeField] private KeyManager keyManager; //열쇠 유무
        [FormerlySerializedAs("dialog")] [SerializeField] private DialogManager dialogManager; //대화

        private void Awake()
        {
            if (nextDoor != null)
            {
                nextDoor.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
            {
                if (keyManager.CheckKey(keyType))
                {
                    dialogManager.DoDialog(new string[]{"문이 열렸다."});
                    if (nextDoor != null)
                    {
                        nextDoor.SetActive(true);
                    }
                    Destroy(gameObject);
                }
                else
                {
                    dialogManager.DoDialog(new string[]{"열쇠가 필요한 것 같다."});
                }
            }
        }
    }
}
