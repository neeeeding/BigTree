using System;
using UnityEngine;

namespace _01Script.Skill
{
    public class ObjCheck : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField]protected LayerMask targetLayer; //감지하길 원하는 레이어들 (전기 나무, 나무, 불, 물, 전기...)
        [SerializeField]protected float radius = 2; //감지 범위
        
        protected LayerMask myLayer; //자신이 뭔지
        protected bool check; // true : 감지함 / false : 감지 못함
        protected Collider[] col; //감지한 애들
        protected bool isStart; // true : 처음 확인 / false : 처음 이후 확인
        
        protected virtual void Awake()
        {
            myLayer = gameObject.layer;
            isStart = true;
        }

        protected virtual void OnDrawGizmos()
        {
            col = Physics.OverlapSphere(gameObject.transform.position, radius, targetLayer);

            check = col.Length > 0;
                
            Gizmos.color = check? Color.red : Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, radius);
        }
    }
}
