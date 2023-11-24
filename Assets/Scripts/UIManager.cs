using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI playerPrimaryCooldownText;
    [SerializeField] private TextMeshProUGUI playerSecondaryCooldownText;
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider enemyHealth;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    private List<PowerUp> playerBoosts;
    [SerializeField] private TextMeshProUGUI playerBoostText;
    private List<PowerUp> enemyBoosts;
    [SerializeField] private TextMeshProUGUI enemyBoostText;
    [SerializeField] private TextMeshProUGUI matchTimeText;

    private string ParseCooldown(float value, float threshold)
    {
        return Math.Max(threshold, Math.Ceiling(value)).ToString(CultureInfo.InvariantCulture);
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
    
    protected override void Awake()
    {
        base.Awake();
        GameEvents.onPowerUpPick += HandlePowerUpPick;
        playerBoosts = new List<PowerUp>();
        enemyBoosts = new List<PowerUp>();
    }
    
    private void Update()
    {
        playerPrimaryCooldownText.text = ParseCooldown(GameManager.Instance.playerController.primaryActualCooldown, 0);
        playerSecondaryCooldownText.text = ParseCooldown(GameManager.Instance.playerController.secondaryActualCooldown, 0);
        playerHealth.value = GameManager.Instance.player.health / 100;
        enemyHealth.value = GameManager.Instance.enemy.health / 100;
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
    }
}
