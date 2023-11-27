using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private TextMeshProUGUI playerPrimaryCooldownText;
    [SerializeField] private GameObject playerPrimaryCooldownImage;
    [SerializeField] private TextMeshProUGUI playerSecondaryCooldownText;
    [SerializeField] private GameObject playerSecondaryCooldownImage;
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider enemyHealth;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    private List<PowerUp> playerBoosts;
    [SerializeField] private TextMeshProUGUI playerBoostText;
    private List<PowerUp> enemyBoosts;
    [SerializeField] private TextMeshProUGUI enemyBoostText;
    [SerializeField] private TextMeshProUGUI matchTimeText;

    private float playerMaxHealth;
    private float enemyMaxHealth;
    
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;

    private GameObject[] panels;

    private static string winningText = "You won";
    private static string oddText = "Odd";
    private static string lostText = "You lost";

    private string ParseCooldown(float value, float threshold)
    {
        return Math.Max(threshold, Math.Ceiling(value)).ToString(CultureInfo.InvariantCulture);
    }

    private void HandleCooldownIcon(TextMeshProUGUI text, GameObject image, float cooldown, float threshold)
    {
        if (cooldown > threshold)
        {
            text.text = ParseCooldown(cooldown, threshold);
            text.gameObject.SetActive(true);
            image.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(false);
            image.SetActive(true);
        }
    }
    
    private string ParseBoosts(List<PowerUp> powerUps)
    {
        string result = "";
        foreach (PowerUp powerUp in powerUps)
        {
            string boostValue = powerUp.boostValue.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
            string rawType = powerUp.type.ToString();
            string type = rawType.Substring(0, 1).ToUpper() + rawType.Substring(1).ToLower();
            result += $"{boostValue} x {type}" + "\n";
        }

        return result;
    }
    
    private IEnumerator RemoveBoost(List<PowerUp> powerUps, PowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.timeValue);
        powerUps.Remove(powerUp);
    }

    private void HandlePowerUpPick(PowerUp powerUp, Collider other)
    {
        Debug.LogFormat("PowerUp {0} picked by {1} with time {2}", powerUp.type, other.tag, powerUp.timeValue);
        if (powerUp.timeValue == 0) return;
        
        List<PowerUp> powerUps = other.CompareTag("Player") ? playerBoosts : enemyBoosts;
        powerUps.Add(powerUp);
        StartCoroutine(RemoveBoost(powerUps, powerUp));
    }

    private void HandlePlay()
    {
        GeneralUI.SetActivePanel(panels, uiPanel);
    }

    private void HandlePause()
    {
        GeneralUI.SetActivePanel(panels, pausePanel);
    }

    public void HandlePausePress()
    {
        GameEvents.GamePlay();
    }

    public void HandleGameEndPress()
    {
        StartCoroutine(LoadScene(0));
    }

    private void HandleEnd(EndGameWinner winner)
    {
        GeneralUI.SetActivePanel(panels, endGamePanel);
        switch (winner)
        {
            case EndGameWinner.NONE:
                endGameText.text = oddText;
                break;
            case EndGameWinner.PLAYER:
                endGameText.text = winningText;
                break;
            case EndGameWinner.ENEMY:
                endGameText.text = lostText;
                break;
        }
    }
    
    private IEnumerator LoadScene(int indexScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);
        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        GameEvents.onPowerUpPick += HandlePowerUpPick;
        GameEvents.onPlay += HandlePlay;
        GameEvents.onPause += HandlePause;
        GameEvents.onEnd += HandleEnd;
        
        playerBoosts = new List<PowerUp>();
        enemyBoosts = new List<PowerUp>();

        panels = new GameObject[] { uiPanel, pausePanel, endGamePanel, loadingPanel };
    }

    private void Start()
    {
        playerMaxHealth = GameManager.Instance.player.health;
        enemyMaxHealth = GameManager.Instance.enemy.health;
    }

    private void Update()
    {
        HandleCooldownIcon(playerPrimaryCooldownText, playerPrimaryCooldownImage, GameManager.Instance.playerController.primaryActualCooldown, 0);
        HandleCooldownIcon(playerSecondaryCooldownText, playerSecondaryCooldownImage, GameManager.Instance.playerController.secondaryActualCooldown, 0);
        playerHealth.value = GameManager.Instance.player.health / playerMaxHealth;
        enemyHealth.value = GameManager.Instance.enemy.health / enemyMaxHealth;
        playerHealthText.text = GameManager.Instance.player.health.ToString(CultureInfo.InvariantCulture);
        enemyHealthText.text = GameManager.Instance.enemy.health.ToString(CultureInfo.InvariantCulture);
        playerBoostText.text = ParseBoosts(playerBoosts);
        enemyBoostText.text = ParseBoosts(enemyBoosts);

        string minutes = Math.Floor(GameManager.Instance.matchTime / 60).ToString(CultureInfo.InvariantCulture);
        string seconds = Math.Floor(GameManager.Instance.matchTime % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
        matchTimeText.text = $"{minutes}:{seconds}";
    }
    
    private void OnDestroy()
    {
        GameEvents.onPowerUpPick -= HandlePowerUpPick;
        GameEvents.onPlay -= HandlePlay;
        GameEvents.onPause -= HandlePause;
        GameEvents.onEnd -= HandleEnd;
    }
}
