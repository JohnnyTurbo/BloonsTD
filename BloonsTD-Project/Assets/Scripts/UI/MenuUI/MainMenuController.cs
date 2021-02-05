using TMG.BloonsTD.Gameplay;
using UnityEngine;

namespace UI.MenuUI
{
    public class MainMenuController : MonoBehaviour
    {
        public void OnButtonNewGame()
        {
            GameController.Instance.SetupNewGame();
            gameObject.SetActive(false);
        }

        public void OnButtonMoreGames()
        {
            Debug.Log("Moar gamez!");   
        }

        public void OnButtonBloonsWorld()
        {
            Debug.Log("Blooz werld!");
        }
    }
}