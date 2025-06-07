using System;
using System.Collections.Generic;
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
        

        private List<ParticleSystem> _attackEffects; //지금 실행할 공격들
        private Dictionary<ParticleSystem, GameObject> _skill; //스킬
        private bool _setting; //true : 시작 / false : 세팅 전


        private void Awake()
        {
            _setting = false;
            _attackEffects =  new List<ParticleSystem>();
            _attackEffects.Add(fireEffect);
            _attackEffects.Add(waterEffect);
            _attackEffects.Add(electricityEffect);
            
            _skill = new Dictionary<ParticleSystem, GameObject>();
            _skill.Add(fireEffect, fire);
            _skill.Add(waterEffect, water);
            _skill.Add(electricityEffect, electricity);
            
            StopAttack();
        }

        private void Update()
        {
            if (_setting&&_attackEffects != null && _attackEffects.Count > 0) //공격 함?
            {

                foreach (ParticleSystem effect in _attackEffects)
                {
                    effect.gameObject.transform.position = Vector3.MoveTowards(
                        effect.gameObject.transform.position,
                        attackPos.position,
                        speed* Time.deltaTime);
                }
                
                float di = Vector3.Distance(_attackEffects[0].gameObject.transform.position, attackPos.position);
                if (di < 0.5f)
                {
                    StopAttack();
                }
            }
        }

        public void CanAttacck(bool f, bool w, bool e)
        {
            _canFire = f;
            _canWater = w;
            _canElectricity = e;
        }

        public void AttackEffect( Vector3 mousePos)  //공격 실행
        {
            _setting = true;
            attackPos.position = mousePos; //목표 지점
            
            if (_canFire)
            {
                _attackEffects.Add(fireEffect);
            }
        
            if (_canWater)
            {
                _attackEffects.Add(waterEffect);
            }

            if (_canElectricity)
            {
                _attackEffects.Add(electricityEffect);
            }

            foreach (ParticleSystem effect in _attackEffects) //실행
            {
                effect.gameObject.transform.localRotation = Quaternion.LookRotation(mousePos);
                effect.gameObject.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
                
                effect.gameObject.SetActive(true);
                effect.Play();
                effect.gameObject.transform.SetParent(attackPos, true);
            }
        }

        public void StopAttack() //공격 멈추기 (이펙트
        {
            
            foreach (ParticleSystem effect in _attackEffects)
            {
                effect.Stop();
                effect.gameObject.SetActive(false);
                effect.gameObject.transform.SetParent(startPos, false);
                
                if(!_setting)
                    continue;
                
                //생성
                GameObject sk = Instantiate(_skill[effect]);
                sk.SetActive(true);
                sk.transform.SetParent(skillPos, false);
                sk.transform.position = attackPos.position;
            }
            
            _attackEffects.Clear();
        }
    }
}
