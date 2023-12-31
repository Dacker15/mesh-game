using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

   [SerializeField] private GameObject loadingPanel;
   [SerializeField] private Slider loadingSlider;

   [SerializeField] private AudioSource musicAudioSource;
   
   private GameObject[] panels;

   protected override void Awake()
   {
      base.Awake();
      panels = new GameObject[] { 
         mainMenuPanel, gameSelectionPanel, tutorialSectionPanel, cubeTutorialSectionPanel, 
         sphereTutorialSectionPanel, powerUpTutorialSectionPanel, creditPanel, loadingPanel,
      };
   }
   private IEnumerator LoadScene(int indexScene)
   {
      GeneralUI.SetActivePanel(panels, loadingPanel);
      musicAudioSource.Pause();
      yield return new WaitForSeconds(0.5f);
      AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);
      while (operation.isDone == false)
      {
         float progress = Mathf.Clamp01(operation.progress / .9f);
         loadingSlider.value = progress;
         yield return null;
      }
   }

   public void HandleStart()
   {
      GeneralUI.SetActivePanel(panels, gameSelectionPanel);
   }

   public void HandleGamePlayerSelection(int type)
   {
      // Handle 0 for cube, 1 for sphere. Enums are not supported
      GameSettings.Instance.playerType = type;
      StartCoroutine(LoadScene(gameScene));
   }

   public void HandleMenu()
   {
      GeneralUI.SetActivePanel(panels, mainMenuPanel);
   }

   public void HandleTutorial()
   {
      GeneralUI.SetActivePanel(panels, tutorialSectionPanel);
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
      GeneralUI.SetActivePanel(panels, panel);
   }

   public void HandleCredits()
   {
      GeneralUI.SetActivePanel(panels, creditPanel);
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
