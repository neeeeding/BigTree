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

        public void GetKey(CheckKeyType v) //열쇠 얻음
        {
            switch (v)
            {
                case CheckKeyType.Water:
                    _water = true;
                    break;
                case CheckKeyType.Fire:
                    _fire = true;
                    break;
                case CheckKeyType.Electricity:
                    _electricity = true;
                    break;
                default:
                    break;
            }
        }

        public enum CheckKeyType
        {
            Water,
            Fire,
            Electricity
        }

        public bool CheckKey(CheckKeyType v)
        {
            return v switch
            {
                CheckKeyType.Water => _water,
                CheckKeyType.Fire => _fire,
                CheckKeyType.Electricity => _electricity,
                _ => false
            };
        }
    }
}