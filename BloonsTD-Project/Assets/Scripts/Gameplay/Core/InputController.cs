using UnityEngine;
using UnityEngine.EventSystems;

namespace TMG.BloonsTD.Gameplay
{
    //TODO Make options for different platforms
    public static class InputController
    {
        private static Camera _mainCamera;
        private static bool _hasMainCamera;
        
        public static bool BeginPlaceTower => Input.GetMouseButtonDown(0);

        public static bool BeginSelectScreen =>
            !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0);

        public static bool BeginCancelSelection => Input.GetKeyDown(KeyCode.Escape);
        public static void ResetMainCamera()
        {
            _hasMainCamera = false;
        }
        private static Camera MainCamera
        {
            get
            {
                if (!_hasMainCamera)
                {
                    _mainCamera = Camera.main;
                    _hasMainCamera = true;
                }

                return _mainCamera;
            }
        }

        public static Vector2 TowerPlacementPosition => MainCamera.ScreenToWorldPoint(Input.mousePosition);

        public static Vector2 WorldSelectionPosition => MainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}