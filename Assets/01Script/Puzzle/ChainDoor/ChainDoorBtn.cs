using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _01Script.Puzzle.ChainDoor
{
    public class ChainDoorBtn : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private int min = 1; //문의 최소 위치
        [SerializeField] private int max = 5; //문의 최대 위치
        [SerializeField] private Vector2 doorSize; // 문 크기
        [SerializeField] private bool isX = false; // true : 가로로 이동 / false : 세로로 이동

        [Header("Need")]
        [SerializeField] private SerializedDictionary<GameObject,int> doors; // 문(?) 들, 위치
        [SerializeField] private SerializedDictionary<GameObject,int[]> btns; // 버튼들
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }
        }
        private void OnMouseDown() //마우스 클릭하면 레이 쏘기
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && btns!=null && btns.Count > 0 && btns.ContainsKey(hit.transform.gameObject))
            { 
                DoorsMove(hit.transform.gameObject);
            }
        }

        private void DoorsMove(GameObject btn)
        {
            // 순회할 키만 따로 리스트로 복사
            var doorList = new List<GameObject>(doors.Keys);
            int curNum = 0;

            foreach (GameObject door in doorList)
            {
                int wantDoorPos = btns[btn][curNum];

                doors[door] += wantDoorPos; // 값 변경

                int difference = DoorMaxMinPos(door, doors[door]);
                wantDoorPos = difference != 0? difference : wantDoorPos; // 또 값 변경 가능
                
                if (isX && difference == 0 )
                {
                    door.transform.localPosition += door.transform.right * doorSize.x * wantDoorPos;
                }
                else if( difference == 0)
                {
                    door.transform.localPosition += door.transform.up * doorSize.y * wantDoorPos;
                }
                
                curNum++;
            }
        }


        private int DoorMaxMinPos(GameObject door, int curPos) //문의 최대, 최소 위치
        {
            int difference = 0;
            if (curPos < min)
            {
                difference = min - curPos;
                doors[door] = min;
            }

            if (curPos > max)
            {
                difference = curPos - max;
                doors[door] = max;
            }

            return difference;
        }
    
    }
}
