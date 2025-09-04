using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Attack,
    Health,
    MoveSpeed,
    SpeedModifier,
    JumpPower
}

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float attack;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [field: SerializeField] public float MoveSpeed;
    [field: SerializeField] public float SpeedModifier { get; set; }
    [field: SerializeField] public float SpeedModifierInput { get; set; }
    [field: SerializeField] public float JumpPower { get; set; }
    //[field: SerializeField] public float KnockbackPower { get; set; }
    [field: SerializeField] public bool CanJump { get; set; }
    [field: SerializeField] public int Gauge { get; set; }
    private Dictionary<StatType, float> stats = new();
    private string userName;

    private void Awake()
    {
        InitStats();
    }

    public void InitStats()
    {
        CharacterManager.Instance.PlayerStat = this;
        userName = gameObject.name;
        stats.Add(StatType.Attack, attack); 
        stats.Add(StatType.Health, currentHealth);
        stats.Add(StatType.MoveSpeed, MoveSpeed);
        stats.Add(StatType.SpeedModifier, SpeedModifier);
        stats.Add(StatType.JumpPower, JumpPower);
    }

    public float GetStat(StatType statType)
    {
        return stats[statType];
    }

    public void TakeDamage()
    {
        Debug.Log("데미지 입음");
        Debug.Log($"기존 체력: {currentHealth}");
        currentHealth -= 1;
        Debug.Log($"바뀐 체력: {currentHealth}");

        if (currentHealth <= 0)
            currentHealth = 0;
    }
}
