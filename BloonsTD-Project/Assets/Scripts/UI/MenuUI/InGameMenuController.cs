using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MenuUI
{
    public class InGameMenuController : MonoBehaviour
    {
        public void OnButtonRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnButtonMoreGames()
        {
            Debug.Log("More Games!");
        }

        public void OnButtonMute()
        {
            Debug.Log("Mute!");
        }
    }
}