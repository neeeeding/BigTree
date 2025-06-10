using System;
using _01Script.Manager;
using _01Script.ObjUI;
using _01Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Obj
{
    public class Door : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private KeyManager.ElementType keyType; //열쇠 종류
        [SerializeField] private GameObject nextDoor;
        [Header("Need")]
        [SerializeField] private KeyManager keyManager; //열쇠 유무
        [SerializeField] private InteractionUI fUi; //상호작용 UI
        [SerializeField] private GameObject door; //열릴 문

        private void Awake()
        {
            if (nextDoor != null)
            {
                nextDoor.SetActive(false);
            }
        }

        private void Update()
        {
            if (keyManager.CheckKey(keyType)) //열쇠를 얻었음
            {
                fUi.ChangeWords(new String[]{"문이 열렸다."});
                if (fUi.IsYou(gameObject)) //지금 문을 상호작용 중일 때
                {
                    if (nextDoor)
                    {
                        nextDoor.SetActive(true);
                    }
                    Destroy(door);
                }
            }
        }
    }
}
