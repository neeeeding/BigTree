using System;
using System.Collections.Generic;
using _01Script.Manager;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Player
{
    public class Attack : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float speed = 4; //이펙트 이동 속도
        [Header("Need")]
        [SerializeField] private Transform startPos; //저장소
        [SerializeField] private Transform attackPos; //공격하는 위치
        [SerializeField] private Transform skillPos; //스킬 위치
    
        //오브제
        [SerializeField] private GameObject fire; //불
        [SerializeField] private GameObject water; //물
        [SerializeField] private GameObject electricity; //전기
        //이펙트
        [SerializeField] private ParticleSystem fireEffect; //불
        [SerializeField] private ParticleSystem waterEffect; //물
        [SerializeField] private ParticleSystem electricityEffect; //전기

        //가능한 공격
        private bool _canFire;
        private bool _canWater;
        private bool _canElectricity;
        
        private bool _setting; //true : 시작 / false : 세팅 전
        
        private ParticleSystem _currentEffect; //지금 공격(위치 정해주기)
        private Dictionary<KeyManager.ElementType, GameObject> _skill; //스킬
        private Dictionary<ParticleSystem, KeyManager.ElementType> _nowAttack; //현재 공격들(실제)
        private Dictionary<ParticleSystem, Vector3> _attackPos; //공격들 위치


        private void Awake()
        {
            _setting = false;

            _attackPos =  new Dictionary<ParticleSystem, Vector3>();
            _nowAttack = new Dictionary<ParticleSystem, KeyManager.ElementType>();
            _skill = new Dictionary<KeyManager.ElementType, GameObject>();
            _skill.Add(KeyManager.ElementType.Fire, fire);
            _skill.Add(KeyManager.ElementType.Water,water);
            _skill.Add(KeyManager.ElementType.Electricity, electricity);
        }

        private void Update()
        {
            if (_setting&&_nowAttack.Count > 0) //공격 함?
            {
                var attackKeys = new List<ParticleSystem>(_nowAttack.Keys);
                for (int i = attackKeys.Count - 1; i >= 0; i--)
                {
                    ParticleSystem att = attackKeys[i];
            
                    // 해당 키가 여전히 Dictionary에 존재하는지 확인
                    if (!_nowAttack.ContainsKey(att)) continue;
            
                    att.gameObject.transform.position = Vector3.MoveTowards(
                        att.gameObject.transform.position,
                        _attackPos[att],
                        speed * Time.deltaTime);
            
                    float di = Vector3.Distance(att.gameObject.transform.position, _attackPos[att]);
                    if (di < 0.5f)
                    {
                        StopAttack(att);
                    }
                }
            }
        }

        public void CanAttacck(bool f, bool w, bool e) //가능한 공격 상태 (받기)
        {
            _canFire = f;
            _canWater = w;
            _canElectricity = e;
        }

        public void AttackEffect( Vector3 mousePos)  //공격 실행
        {
            _setting = true;
            ParticleSystem effect;
            KeyManager.ElementType  type = KeyManager.ElementType.None;
            
            if (_canFire)
            {
                effect = fireEffect;
                type = KeyManager.ElementType.Fire;
            }
            else if (_canWater)
            {
                effect = waterEffect;
                type = KeyManager.ElementType.Water;
            }
            else if (_canElectricity)
            {
                effect = electricityEffect;
                type = KeyManager.ElementType.Electricity;
            }
            else
            {
                effect = null;
                type = KeyManager.ElementType.None;
            }

            if (effect)
            {
                ParticleSystem att = Instantiate(effect, transform);
                att.gameObject.transform.localRotation = Quaternion.LookRotation(mousePos);
                att.gameObject.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
                
                att.gameObject.SetActive(true);
                att.Play();
                att.gameObject.transform.SetParent(attackPos, true);

                _currentEffect = att;
                _nowAttack.Add(att,type );
                _attackPos.Add(_currentEffect,mousePos); //목표 지점
            }
        }

        private void StopAttack(ParticleSystem att) //공격 멈추기 (이펙트
        {
            if (att)
            {
                //생성
                KeyManager.ElementType type = _nowAttack[att];
                GameObject sk = Instantiate(_skill[type]);
                sk.SetActive(true);
                sk.transform.SetParent(skillPos, false);
                sk.transform.position = _attackPos[att];
                
                
                Destroy(att.gameObject);
                _nowAttack.Remove(att);
            }
        }
    }
}
