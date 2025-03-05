using System.Collections;
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
        public const string ModName = "Client Host info";
        public const string ModVersion = "1.0.2";
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
            MelonCoroutines.Start(InitializeMapTextBoxes(1f));
        }

        private IEnumerator InitializeMapTextBoxes(float delay)
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
        private IEnumerator MoveInfoPanelAfterDelay(float delay,Vector3 vector3, Quaternion quaternion)
        {
            
            yield return new WaitForSeconds(delay);

            if (_infoPanel == null)
            {
                MelonLogger.Warning("Info panel is null");
                yield break; // Exit the coroutine if no panel exists
            }

            _infoPanel.transform.position = vector3;
            _infoPanel.transform.rotation = quaternion;
        }

        private void Map0Init()
        {
            if (Calls.Players.IsHost())
            {
                _infoPanel = CreateTextBox("Host", 10f, Color.yellow, 
                    new Vector3(0f, 3.75f, -11f), Quaternion.Euler(0, 183f, 0)); 
            }
            else
            {
                _infoPanel = CreateTextBox("Client", 10f, Color.yellow, 
                    new Vector3(0f, 3.75f, 11f), Quaternion.Euler(0, 12, 0));
            }
            
            MelonCoroutines.Start(MoveInfoPanelAfterDelay(5f,new Vector3(0f,2f,-18f),
                Quaternion.Euler(0, 183f, 0))); // Start coroutine to disable the info panel
        }

        private void Map1Init()
        {
            if (Calls.Players.IsHost())
            {
                _infoPanel = CreateTextBox("Host", 10f, Color.yellow, 
                    new Vector3(0 ,3.75f, -9), Quaternion.Euler(0, 183f, 0));
            }
            else
            {
                _infoPanel = CreateTextBox("Client", 10f, Color.yellow, 
                    new Vector3(0 ,3.75f, 9), Quaternion.Euler(0, 12, 0));
            }
            
            MelonCoroutines.Start(MoveInfoPanelAfterDelay(5f,new Vector3(0 ,3.75f, -10.7f),
                Quaternion.Euler(0, 183f, 0))); // Start coroutine to disable the info panel
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