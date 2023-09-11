using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Broccollie.System
{
    public class AssetSceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetSceneEventChannel _eventChannel;

        private void OnEnable()
        {
            _eventChannel.OnSceneLoadAsync += SceneLoadAsync;
        }

        private void OnDisable()
        {
            _eventChannel.OnSceneLoadAsync -= SceneLoadAsync;
        }

        #region Subscribers
        private async Task SceneLoadAsync(AssetScenePreset scene)
        {
            await UnloadSceneAsync();
            await LoadSceneAsync(scene);
        }

        #endregion

        #region Public Functions
        public async Task UnloadSceneAsync()
        {
            try
            {
                UnityEngine.SceneManagement.Scene activeScene = SceneManager.GetActiveScene();
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(activeScene);
                if (unloadOperation == null) return;

                while (!unloadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(unloadOperation.progress / 0.9f);
                    //Debug.Log($"[ SceneLoader ] Unload progress: {progress}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async Task LoadSceneAsync(AssetScenePreset scene)
        {
            try
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene.SceneName);
                if (loadOperation == null) return;

                while (!loadOperation.isDone)
                {
                    float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    //Debug.Log($"[ SceneLoader ] Load progress: {progress}");
                    await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #endregion
    }
}
