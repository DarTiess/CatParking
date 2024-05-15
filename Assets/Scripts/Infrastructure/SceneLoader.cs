using DG.Tweening;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private IEventBus _eventBus;

        public SceneLoader(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<RestartScene>(OnRestartScene);
            _eventBus.Subscribe<Win>(OnWinLevel);
            _eventBus.Subscribe<Failed>(OnFailedLevel);
        }

        private void OnFailedLevel(Failed obj)
        {
            DOVirtual.DelayedCall(1.3f, () =>
            {
               RestartLevel();
            });
        }

        private void OnWinLevel(Win obj)
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            DOVirtual.DelayedCall(1.3f, () =>
            {
                LoadNextLevel(nextScene);
            });
        }

        private void LoadNextLevel(int nextScene)
        {
            if (nextScene < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                RestartLevel();
            }
        }

        private void RestartLevel()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }

        private void OnRestartScene(RestartScene obj)
        {
           RestartLevel();
        }
    }
}