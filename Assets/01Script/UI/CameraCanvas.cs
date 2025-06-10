using UnityEngine;

namespace _01Script.UI
{
    public class CameraCanvas : MonoBehaviour
    {

        private Camera _camera; //카메라
        private float _scaleheight; //
        private float _scalewidth;
        private Rect _rect; // 사이즈
        private void Start()
        {
            _camera = Camera.main;
            _rect = _camera.rect;
            _scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); 
            _scalewidth  = 1f / _scaleheight;
            if (_scaleheight < 1)
            {
                _rect.height = _scaleheight;
                _rect.y = (1f - _scaleheight) / 2f;
            }
            else
            {
                _rect.width = _scalewidth;
                _rect.x = (1f - _scalewidth) / 2f;
            }
            _camera.rect = _rect;
        }

        void OnPreCull() => GL.Clear(true, true, Color.black);
    }
}