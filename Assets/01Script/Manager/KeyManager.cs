using System;
using UnityEngine;

namespace _01Script.Manager
{
    public class KeyManager : MonoBehaviour
    {
        //각 속성 키
        private bool _water;
        private bool _fire;
        private bool _electricity;

        private void Awake()
        {
            _water = false;
            _fire = false;
            _electricity = false;
        }

        public void GetKey(ElementType v) //열쇠 얻음
        {
            switch (v)
            {
                case ElementType.Water:
                    _water = true;
                    break;
                case ElementType.Fire:
                    _fire = true;
                    break;
                case ElementType.Electricity:
                    _electricity = true;
                    break;
                default:
                    break;
            }
        }

        public enum ElementType
        {
            Water,
            Fire,
            Electricity,
            None
        }

        public bool CheckKey(ElementType v)
        {
            return v switch
            {
                ElementType.Water => _water,
                ElementType.Fire => _fire,
                ElementType.Electricity => _electricity,
                _ => false
            };
        }
    }
}