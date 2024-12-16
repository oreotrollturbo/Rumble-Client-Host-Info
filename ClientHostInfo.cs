using System.Collections;
using Il2CppSystem.Runtime.ConstrainedExecution;
using MelonLoader;
using BuildInfo = ClientHostInfo.BuildInfo;
using RumbleModdingAPI;
using UnityEngine;


[assembly: MelonInfo(typeof(ClientHostInfo.TestMod), BuildInfo.ModName, BuildInfo.ModVersion, BuildInfo.Author)]
[assembly: MelonGame("Buckethead Entertainment", "RUMBLE")]

namespace ClientHostInfo
{
    public static class BuildInfo
    {
        public const string ModName = "Host info";
        public const string ModVersion = "1.0";
        public const string Author = "oreotrollturbo";
    }

    public class TestMod : MelonMod
    {
        private static string? _currentSceneName;

        private GameObject? _infoPanel;

        public override void OnLateInitializeMelon()
        {
            Calls.onMapInitialized += SceneLoaded;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _currentSceneName = sceneName;
        }

        private void SceneLoaded()
        {
            if (_currentSceneName is "Map0" or "Map1")
            {
                MelonCoroutines.Start(DisableInfoPanelAfterDelay(5f)); // Start coroutine to disable the info panel
            }

            // Start a coroutine to wait 2 seconds before executing the switch logic
            MelonCoroutines.Start(WaitAndHandleSceneSwitch(1f));
        }

        private IEnumerator WaitAndHandleSceneSwitch(float delay)
        {
            yield return new WaitForSeconds(delay);

            switch (_currentSceneName)
            {
                // case "Gym":
                //     GymInit();
                //     break;

                case "Map0":
                    Map0Init();
                    break;

                case "Map1":
                    Map1Init();
                    break;
            }
        }
        private IEnumerator DisableInfoPanelAfterDelay(float delay)
        {
            
            yield return new WaitForSeconds(delay);

            if (_infoPanel == null)
            {
                MelonLogger.Warning("Info panel is null");
                yield break; // Exit the coroutine if no panel exists
            }

            _infoPanel.SetActive(false);
        }

        // private void GymInit()
        // { 
        //     CreateTextBox("Hello world", 10f, Color.blue, new Vector3(5.5236f, 2.75f, 11.3364f), Quaternion.Euler(0, 30.5f, 0));
        //     
        //     Calls.Create.NewButton(new Vector3(2.4795f, 0.4392f, -3.5319f), new Quaternion(0, 0f, 0, 0));
        // }

        private void Map0Init()
        {
            if (Calls.Players.IsHost())
            {
                _infoPanel = CreateTextBox("Host", 10f, Color.yellow,
                    new Vector3(0f, 2.75f, -11f), Quaternion.Euler(0, 183f, 0)); 

                // _infoPanel.transform.parent = (GameObject.Find("/Health").transform);

                MelonLogger.Warning("Text position: " + _infoPanel.transform.localPosition);
                MelonLogger.Warning("Scene: " + _currentSceneName);
            }
            else
            {
                _infoPanel = CreateTextBox("Client", 10f, Color.yellow, 
                    new Vector3(0f, 2.75f, 11f), Quaternion.Euler(0, 12, 0));
                
                // _infoPanel.transform.parent = (GameObject.Find("/Health").transform);
                
                MelonLogger.Warning("Text position: " + _infoPanel.transform.localPosition);
                MelonLogger.Warning("Scene: " + _currentSceneName);
            }
        }

        private void Map1Init()
        {
            if (Calls.Players.IsHost())
            {
                _infoPanel = CreateTextBox("Host", 10f, Color.yellow, 
                    new Vector3(0 ,3, -9), Quaternion.Euler(0, 30.5f, 0));
            }
            else
            {
                _infoPanel = CreateTextBox("Client", 10f, Color.yellow, 
                    new Vector3(0 ,3, 9), Quaternion.Euler(0, 12, 0));
            }
        }


        private static GameObject CreateTextBox(String text,float textSize , Color color , Vector3 vector, Quaternion rotation)
        {
            GameObject gameObject = Calls.Create.NewText(text, textSize, color, 
                new Vector3(), Quaternion.Euler(0,0,0));

            gameObject.transform.position = vector;
            gameObject.transform.rotation = rotation;

            return gameObject;

        }
    }
}