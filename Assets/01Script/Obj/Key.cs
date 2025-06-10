using _01Script.Manager;
using _01Script.ObjUI;
using _01Script.UI;
using UnityEngine;

namespace _01Script.Obj
{
    public class Key : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private KeyManager.ElementType keyType; //열쇠 종류
        [Header("Need")]
        [SerializeField] private KeyManager key; //열쇠 얻기
        [SerializeField] private InteractionUI fUi; //상호작용 UI
        
        private void Update()
        {
            if (fUi.IsYou(gameObject))
            {
                key.GetKey(keyType);
                Destroy(gameObject);
            }
        }
    }
}