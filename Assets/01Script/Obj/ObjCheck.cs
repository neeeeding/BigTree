using UnityEngine;

namespace _01Script.Obj
{
    public class ObjCheck : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField]protected LayerMask targetLayer; //감지하길 원하는 레이어들 (전기 나무, 나무, 불, 물, 전기...)
        [SerializeField]protected float radius = 2; //감지 범위
        
        protected LayerMask MyLayer; //자신이 뭔지
        protected bool Check; // true : 감지함 / false : 감지 못함
        protected Collider[] Col; //감지한 애들
        protected bool IsStart; // true : 처음 확인 / false : 처음 이후 확인
        
        protected virtual void Awake()
        {
            MyLayer = gameObject.layer;
            IsStart = true;
        }

        protected virtual void OnDrawGizmos()
        {
            Col = Physics.OverlapSphere(gameObject.transform.position, radius, targetLayer);

            Check = Col.Length > 0;
                
            Gizmos.color = Check? Color.red : Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, radius);
        }
    }
}
