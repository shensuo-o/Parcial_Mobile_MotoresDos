using Managers.Menu;
using UnityEngine;

namespace Managers.Level
{
    public class ChangeSceneButton : MonoBehaviour
    {
        public void ChaneScene(string sceneName)
        {
            MenuManager.instance.LoadLevel(sceneName);
        }
    }
}
