using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
    
        [SerializeField] private HintManager hintManager;
        [SerializeField] private GameObject loaderCanvas;
        [SerializeField] private Image progressBar;
        private float _progress;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async void LoadScene(string sceneName = "Menu")
        {
            print("yendo al menu");
            progressBar.fillAmount = 0;
            _progress = 0;
        
            loaderCanvas.SetActive(true);
            hintManager.RandomHint();
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene!.allowSceneActivation = false;
        
            do
            {
                await Task.Delay(100);
            
                _progress = scene.progress;
            
            } while (scene.progress < 0.9f);
        
            await Task.Delay(1000);
        
            scene!.allowSceneActivation = true;
        
            await Task.Delay(1000);
        
            loaderCanvas.SetActive(false);
        }

        private void Update()
        {
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, _progress, 3 * Time.deltaTime);
        }

        public void LoadMenuScene()
        {
            instance.LoadScene();
        }
    }
}
