using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _01Script.Puzzle.ChainDoor
{
    public class ChainDoorBtn : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private int min = 0; //문의 최소 위치
        [SerializeField] private int max = 5; //문의 최대 위치
        [SerializeField] private float doorSize; // 문 높이

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
                door.transform.position += new Vector3(0, doorSize * wantDoorPos, 0);

                doors[door] += wantDoorPos;        // 값 변경
                DoorMaxMinPos(door, doors[door]);   // 또 값 변경 가능
                curNum++;
            }
        }


        private void DoorMaxMinPos(GameObject door, int curPos) //문의 최대, 최소 위치
        {
            if (curPos < min)
            {
                int difference = min - curPos;
                doors[door] = min;
                door.transform.position += new Vector3(0, doorSize * difference, 0);
            }

            if (curPos > max)
            {
                int difference = curPos - max;
                doors[door] = max;
                door.transform.position -= new Vector3(0, doorSize * difference, 0);
            }
        }
    
    }
}
