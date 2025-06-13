using System;
using _01Script.Manager;
using _01Script.Player;
using UnityEngine;

namespace _01Script.Obj
{
    public class Pool : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float delay = 60 * 5; // 비활성화 할 시간

        private float time;
        
        private Attack _parent; //부모
        private KeyManager.ElementType _type;

        private void OnEnable()
        {
            time = 0;
        }

        private void Update()
        {
            if(_parent)
                Hide();
        }

        public void SetParent(Attack attack,  KeyManager.ElementType type)
        {
            _parent = attack;
            _type = type;
        }

        private void Hide()
        {
            time +=  Time.deltaTime;
            if (time >= delay)
            {
                _parent.PoolPush(_type, gameObject);
            }
        }
    }
}