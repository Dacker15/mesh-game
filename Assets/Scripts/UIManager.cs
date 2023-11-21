using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public AttackController player;
    public AttackController enemy;
    [SerializeField] private TextMeshProUGUI playerPrimaryCooldownText;
    [SerializeField] private TextMeshProUGUI playerSecondaryCooldownText;
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider enemyHealth;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI matchTimeText;

    private string ParseCooldown(float value, float threshold)
    {
        return Math.Max(threshold, Math.Ceiling(value)).ToString(CultureInfo.InvariantCulture);
    }
    
    private void Update()
    {
        playerPrimaryCooldownText.text = ParseCooldown(player.primaryActualCooldown, 0);
        playerSecondaryCooldownText.text = ParseCooldown(player.secondaryActualCooldown, 0);
        playerHealth.value = GameManager.Instance.player.health / 100;
        enemyHealth.value = GameManager.Instance.player.health / 100;
        playerHealthText.text = GameManager.Instance.player.health.ToString(CultureInfo.InvariantCulture);
        enemyHealthText.text = GameManager.Instance.enemy.health.ToString(CultureInfo.InvariantCulture);

        string minutes = Math.Floor(GameManager.Instance.matchTime / 60).ToString(CultureInfo.InvariantCulture);
        string seconds = Math.Floor(GameManager.Instance.matchTime % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
        matchTimeText.text = $"{minutes}:{seconds}";
    }
}
