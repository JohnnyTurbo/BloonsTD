using UnityEngine;

namespace TMG.BloonsTD.Gameplay
{
    //TODO Make options for different platforms
    public static class InputController
    {
        private static Camera _mainCamera;
        
        public static bool PlaceTowerFlag => Input.GetMouseButtonDown(0);
        public static bool ScreenSelectionFlag => Input.GetMouseButtonDown(0);
        public static bool CancelSelection => Input.GetKeyDown(KeyCode.Escape);
        public static Vector2 TowerPlacementPosition
        {
            get
            {
                if(_mainCamera == null){_mainCamera = Camera.main;}
                return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        public static Vector2 WorldSelectionPosition
        {
            get
            {
                if(_mainCamera == null){_mainCamera = Camera.main;}
                return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}