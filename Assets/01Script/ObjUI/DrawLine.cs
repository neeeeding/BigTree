using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [Header("Need")]
    [SerializeField] private GameObject targetObject; //(콘센트)오브제
    [SerializeField] private LineRenderer line; //줄
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && MousePos())
        {
            FollowLine();
        }
    }

    private void FollowLine()
    {
        targetObject.transform.position = Input.mousePosition;
        
        
    }
    
    private bool MousePos() //마우스가 전선 눌렀는지
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == targetObject;
        }
        
        return false;
    }
}
