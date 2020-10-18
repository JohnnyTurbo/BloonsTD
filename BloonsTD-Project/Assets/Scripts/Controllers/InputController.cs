using System;
using UnityEngine;

namespace TMG.BloonsTD.Controllers
{
    public static class InputController
    {
        public static bool ReadPlaceTower()
        {
            //TODO Make options for different platforms

            return Input.GetMouseButtonDown(0);
        }
    }
}