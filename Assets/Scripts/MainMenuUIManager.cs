using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
   [SerializeField] private int gameScene;

   [SerializeField] private GameObject mainMenuPanel;
   [SerializeField] private GameObject gameSelectionPanel;
   
   private IEnumerator LoadScene(int indexScene)
   {
      AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);
      while (operation.isDone == false)
      {
         // float progress = Mathf.Clamp01(operation.progress / .9f);
         yield return null;
      }
   }

   public void HandleStart()
   {
      mainMenuPanel.SetActive(false);
      gameSelectionPanel.SetActive(true);
   }

   public void HandleGamePlayerSelection(int type)
   {
      // Handle 0 for cube, 1 for sphere. Enums are not supported
      GameSettings.Instance.playerType = type;
      StartCoroutine(LoadScene(gameScene));
   }

   public void HandleGameBack()
   {
      gameSelectionPanel.SetActive(false);
      mainMenuPanel.SetActive(true);
   }

   public void HandleTutorial()
   {
      Debug.Log("Show tutorial panel");
   }

   public void HandleCredits()
   {
      Debug.Log("Show credit panel");
   }

   public void HandleQuit()
   {
      #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
      #else
         Application.Quit();
      #endif
   }
}
