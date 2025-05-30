using UnityEngine;

namespace _01Script.Player.States
{
    [CreateAssetMenu(fileName = "State", menuName = "SO/State",  order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}