using Assets.Scripts.Systems.Entities.Stats;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        private FloatStat playerHealth;
        private TextMeshProUGUI textUI;

        private void Start()
        {
            textUI = GetComponent<TextMeshProUGUI>();
            playerHealth = PlayerController.Instance.EntityStats.Health;
            playerHealth.StatChanged += Health_StatChanged;
            SetPlayerHealth(playerHealth.Value);
        }

        private void SetPlayerHealth(float value)
        {
            textUI.text = $"Player Health: {value}";
        }

        private void Health_StatChanged(Systems.Entities.Stats.ModifiableStat<float> sender, Systems.Entities.Stats.StatChangedEventsArgs<float> e)
        {
            SetPlayerHealth(e.NewValue);
        }
    }
}
