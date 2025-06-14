using System;
using _01Script.Obj;
using _01Script.ObjUI;
using _01Script.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01Script.Turorial
{
    public class Turorial : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private String[] words = 
            {"이것을 좌클릭 해보자.",
                "아까 그것을 드레그 해서\n빨간 버튼과 연결하자.",
                "우클릭으로 화면을 회전해보자. (아래)\n(이동은 W A S D로 가능하다.)", //3
                "물을 찾아서 클릭해보자.",
                "불은 물을 없앨수 있다.\n이번엔 노란 버튼과 연결하자", //5
                "물을 찾아서 클릭해보자.",
                "물에 전기가 닿으면 전기를 품을 수 있다.\n이 전기는 시간이 지나면 사라진다.\n이번엔 파란 버튼과 연결하자", //7
                "불을 찾아서 클릭해보자",
                "마찬가지로 물은 불을 없앨수 있다.\n모든 상자들를 찾아서 물로 연결하고\n전기가 통하게 해보자.", //9
                "상자에 직접적으로 전기는 통할 수 없지만\n물을 통해 전기를 공급할 수 있다\n나무에 물을 연결하고 전기를 사용해보자.",
                "이 나무는 물이 전기가 통하는 것을 막는다.\n다만 근처에 불이 있으면\n이 능력이 상쇄된다.\n다시 시도해보자", //11
                "얼음을 찾아 불로 없애보자.",
                "특정 공간에서는 없앴던 얼음이\n일정 시간 후 다시 생성된다.\nM을 눌러보자.", //13
                "M을 누르면 지도를 볼 수 있다.\n지도에는 각 열쇠 위치 혹은 상자등이 표시 되어 있다.\nF를 눌러보자.", 
                "특정 오브젝트들은 F를 누르면\n상호작용 혹은 설명을 볼수 있다.\nSpace를 눌러보자", //15
                "Shift로 달리고,\nSpace로 점프가 가능하다.",
                "튜토리얼이 끝났다."//17
            };
        
        [Header("Need")]
        [SerializeField] private GameObject objImage; //콘센트만 보여주기
        [SerializeField] private GameObject btnImage; //버튼들만 보여주기
        [SerializeField] private TextMeshProUGUI text; //설명 텍스트
        [Space(10)]
        [SerializeField] private GameObject water; //불에 사라질 물
        [SerializeField] private GameObject eWater; //전기 테스트 물
        [SerializeField] private GameObject fire; //불
        [SerializeField] private GameObject ice; //얼음
        [SerializeField] private GameObject TObj; //기믹들

        [SerializeField]private DrawLine LineObj; // 콘센트
        [SerializeField]private MetalObj box; // 상자
        [SerializeField] private ChangeScene scene;

        private int TurorialCount; //1 : 콘센트 잡아라, 2 : 빨강 연결
    
        private bool f;
        private bool w;
        private bool e;
        
        private bool _isFollow; //

        private bool isOk; //실행 했는가에 대한
        private bool isse; //실행 했는가에 대한(세컨드)

        [ContextMenu("ResetWord")]
        private void ResetWord()
        {
            words = new []
            {"이것을 좌클릭 해보자.",
                "아까 그것을 드레그 해서\n빨간 버튼과 연결하자.",
                "우클릭으로 화면을 회전해보자. (아래)\n(이동은 W A S D로 가능하다.)", //3
                "물을 찾아서 클릭해보자.",
                "불은 물을 없앨수 있다.\n이번엔 노란 버튼과 연결하자", //5
                "물을 찾아서 클릭해보자.",
                "물에 전기가 닿으면 전기를 품을 수 있다.\n이 전기는 시간이 지나면 사라진다.\n이번엔 파란 버튼과 연결하자", //7
                "불을 찾아서 클릭해보자",
                "마찬가지로 물은 불을 없앨수 있다.\n모든 상자들를 찾아서 물로 연결하고\n전기가 통하게 해보자.", //9
                "상자에 직접적으로 전기는 통할 수 없지만\n물을 통해 전기를 공급할 수 있다\n나무에 물을 연결하고 전기를 사용해보자.",
                "이 나무는 물이 전기가 통하는 것을 막는다.\n다만 근처에 불이 있으면\n이 능력이 상쇄된다.\n다시 시도해보자", //11
                "얼음을 찾아 불로 없애보자.",
                "특정 공간에서는 없앴던 얼음이\n일정 시간 후 다시 생성된다.\nM을 눌러보자.", //13
                "M을 누르면 지도를 볼 수 있다.\n지도에는 각 열쇠 위치 혹은 상자등이 표시 되어 있다.\nF를 눌러보자.", 
                "특정 오브젝트들은 F를 누르면\n상호작용 혹은 설명을 볼수 있다.\nSpace를 눌러보자", //15
                "Shift로 달리고,\nSpace로 점프가 가능하다.",
                "튜토리얼이 끝났다."//17
            };

        }

        private void OnEnable()//활성화
        {
            isOk =  false;
            ObjT();
            water.SetActive(false);
            fire.SetActive(false);
            eWater.SetActive(false);
            TObj.SetActive(false);
        }

        private void Update()
        {
            CheckObj();

            switch (TurorialCount) //튜토리얼 단계
            {
                case 1 :
                    if (_isFollow) {BtnT();} //콘센트를 잡음
                    break;
                case 2 :
                    if (f) {DoSkill();} //제대로 연결
                    if (!_isFollow) {ObjT();} //콘센트를 놓음
                    break;
                case 3 ://우클릭 회전
                    if (Input.GetMouseButtonDown(1)) isOk = true;
                    if(Input.GetMouseButtonUp(1) && isOk) DoSkill();
                    break;
                case 4 ://불로 물 삭제
                    if(!water)
                        DoSkill();
                    else water.SetActive(true);
                    break;
                case 5 ://전기로 변경
                    if(e) {DoSkill();}
                    break;
                case 6 :
                    eWater.SetActive(true);
                    if (Input.GetMouseButtonDown(0)) isOk = true;
                    if(Input.GetMouseButtonUp(0) && isOk) DoSkill();
                    break;
                case 7 ://물로 변경
                    if(w) {DoSkill();}
                    break;
                case 8 : //불 없애기
                    if(!fire) DoSkill();
                    else fire.SetActive(true);
                    break;
                case 9 : //상자 연결
                    TObj.SetActive(true);
                    if(box.CheckAll()) DoSkill();
                    break;
                case 10: //나무 연결
                    if(w) isOk = true;
                    if(e && isOk) isse = true;
                    if(Input.GetMouseButtonDown(0) && e &&isse&&isOk) DoSkill();
                    break;
                case 11: //나무 다시 연결
                    if(f) isOk = true;
                    if(e && isOk) isse = true;
                    if(Input.GetMouseButtonDown(0) && e &&isse&&isOk) DoSkill();
                    break;
                case 12 : //얼음 없애기
                    if(!ice) DoSkill();
                    else ice.SetActive(true);
                    break;
                case 13: //지도
                    if(Input.GetKeyDown(KeyCode.M)) DoSkill();
                    break;
                case 14: //상호작용
                    if(Input.GetKeyDown(KeyCode.F)) DoSkill();
                    break;
                case 15: //달리기와 점프
                    if(Input.GetKeyDown(KeyCode.Space)) DoSkill();
                    break;
                case 16: //끝
                    if(Input.anyKeyDown) DoSkill();
                    break;
                case 17: //끝
                    scene.Button("GameScene");
                    break;
                default:
                    break;
            }
        }

        private void ObjT()
        {
            objImage.SetActive(true);
            btnImage.SetActive(false);
            text.gameObject.SetActive(true);
            text.text = words[0];
            TurorialCount = 1;
        }
        private void BtnT()
        {
            objImage.SetActive(false);
            btnImage.SetActive(true);
            text.gameObject.SetActive(true);
            text.text = words[1];
            TurorialCount = 2;
        }

        private void DoSkill() //능력 사용
        {
            isOk = false;
            isse = false;
            TurorialCount++;
            objImage.SetActive(false);
            btnImage.SetActive(false);
            text.gameObject.SetActive(true);
            if (TurorialCount > words.Length)
            {
                return;
            }
            text.text = words[TurorialCount - 1];
        }

        private void CheckObj()
        {
            bool[] re = LineObj.IsFollow();
        
            _isFollow = re[0];
            f = re[1];
            w = re[2];
            e = re[3];
        }
    
    }
}
