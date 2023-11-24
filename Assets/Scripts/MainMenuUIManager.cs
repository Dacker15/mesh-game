using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
   [SerializeField] private int gameScene;

   [SerializeField] private GameObject mainMenuPanel;
   [SerializeField] private GameObject gameSelectionPanel;
   [SerializeField] private GameObject tutorialSectionPanel;
   [SerializeField] private GameObject cubeTutorialSectionPanel;
   [SerializeField] private GameObject sphereTutorialSectionPanel;
   [SerializeField] private GameObject powerUpTutorialSectionPanel;
   [SerializeField] private GameObject creditPanel;
   private GameObject[] panels;

   protected override void Awake()
   {
      base.Awake();
      panels = new GameObject[] { mainMenuPanel, gameSelectionPanel, tutorialSectionPanel, cubeTutorialSectionPanel, sphereTutorialSectionPanel, powerUpTutorialSectionPanel, creditPanel };
   }

   private IEnumerator LoadScene(int indexScene)
   {
      AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);
      while (operation.isDone == false)
      {
         // float progress = Mathf.Clamp01(operation.progress / .9f);
         yield return null;
      }
   }

   private void SetActivePanel(GameObject panel)
   {
      foreach (var currentPanel in panels)
      {
         if (currentPanel == panel)
         {
            currentPanel.SetActive(true);
         }
         else
         {
            currentPanel.SetActive(false);
         }
      }
   }

   public void HandleStart()
   {
      SetActivePanel(gameSelectionPanel);
   }

   public void HandleGamePlayerSelection(int type)
   {
      // Handle 0 for cube, 1 for sphere. Enums are not supported
      GameSettings.Instance.playerType = type;
      StartCoroutine(LoadScene(gameScene));
   }

   public void HandleMenu()
   {
      SetActivePanel(mainMenuPanel);
   }

   public void HandleTutorial()
   {
      SetActivePanel(tutorialSectionPanel);
   }

   public void HandleTutorialSection(int index)
   {
      GameObject panel;
      switch (index)
      {
         case 0:
            panel = cubeTutorialSectionPanel;
            break;
         case 1:
            panel = sphereTutorialSectionPanel;
            break;
         case 2:
         default:
            panel = powerUpTutorialSectionPanel;
            break;
      }
      SetActivePanel(panel);
   }

   public void HandleCredits()
   {
      SetActivePanel(creditPanel);
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
