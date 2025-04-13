using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmartHome.Utils
{
    /// <summary>
    /// Вспомогательные функции. Общий статик класс, отлично подходит для общих функций, которые не относятся к конкретному классу.
    /// </summary>
    public static class Utils
    {
        public static string FormatTime(float timeInSeconds)
        {
            int days = (int)(timeInSeconds / 86400); // 86400 seconds in a day
            int hours = (int)((timeInSeconds % 86400) / 3600);
            int minutes = (int)((timeInSeconds % 3600) / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{days}d {hours}h {minutes}m {seconds}s";
        }

        public static void ChangeTimeScale(float scale)
        {
            Time.timeScale = scale;
        }

        /// <summary>
        /// Загружает все сцены из настроек сборки. Первая сцена загружается как основная, остальные как дополнительные.
        /// </summary>
        public static void LoadAllScenesFromBuildSettings()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 1; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                bool alreadyLoaded = false;

                for (int j = 0; j < SceneManager.sceneCount; j++)
                {
                    Scene loadedScene = SceneManager.GetSceneAt(j);
                    if (loadedScene.name == sceneName)
                    {
                        alreadyLoaded = true;
                        break;
                    }
                }

                if (!alreadyLoaded)
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Полностью перезагружает все открытые сцены в порядке загрузки.
        /// </summary>
        public static void ReloadAllOpenScenes()
        {
            int count = SceneManager.sceneCount;
            string[] paths = new string[count];

            for (int i = 0; i < count; i++)
            {
                paths[i] = SceneManager.GetSceneAt(i).path;
            }

            // Загружаем первую сцену с очисткой
            SceneManager.LoadScene(paths[0], LoadSceneMode.Single);

            // Остальные — добавляем
            for (int i = 1; i < paths.Length; i++)
            {
                SceneManager.LoadScene(paths[i], LoadSceneMode.Additive);
            }
            ChangeTimeScale(1f);
        }

        public static void QuitGame()
        {
            // Check if the application is running in the editor
#if UNITY_EDITOR
            // Stop playing the scene in the editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
                // Quit the application
               UnityEngine.Application.Quit();
#endif
        }
    }
}