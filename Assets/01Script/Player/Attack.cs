using System.Collections.Generic;
using UnityEngine;

namespace _01Script.Player
{
    public class Attack : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private Transform startPos; //저장소
        [SerializeField] private Transform attackPos; //공격하는 위치
    
        [SerializeField] private ParticleSystem fire; //불
        [SerializeField] private ParticleSystem water; //물
        [SerializeField] private ParticleSystem electricity; //전기

        private Dictionary<string, ParticleSystem> attackEffect;

        private void Awake()
        {
            StopAttack();
        }


        public void AttackEffect(bool f, bool w, bool e)
        {
            if (f)
            {
                fire.gameObject.SetActive(true);
                fire.Play();
                fire.gameObject.transform.SetParent(attackPos, false);
            }
        
            if (w)
            {
                water.gameObject.SetActive(true);
                water.Play();
                water.gameObject.transform.SetParent(attackPos, false);
            }

            if (e)
            {
                electricity.gameObject.SetActive(true);
                electricity.Play();
                electricity.gameObject.transform.SetParent(attackPos, false);
            }
        }

        public void StopAttack()
        {
            fire.Stop();
            water.Stop();
            electricity.Stop();
        
            fire.gameObject.SetActive(false);
            water.gameObject.SetActive(false);
            electricity.gameObject.SetActive(false);
        
            fire.gameObject.transform.SetParent(startPos, false);
            fire.gameObject.transform.localPosition = Vector3.zero;
        
            water.gameObject.transform.SetParent(startPos, false);
            water.gameObject.transform.localPosition = Vector3.zero;
        
            electricity.gameObject.transform.SetParent(startPos, false);
            electricity.gameObject.transform.localPosition = Vector3.zero;
        
        
        }
    }
}
