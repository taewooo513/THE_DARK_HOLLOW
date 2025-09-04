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
    [field: SerializeField] public int Attack {  get; set; }
    [SerializeField] private float maxHealth;
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MoveSpeed;
    [field: SerializeField] public float SpeedModifier { get; set; }
    [field: SerializeField] public float SpeedModifierInput { get; set; }
    [field: SerializeField] public float JumpPower { get; set; }
    //[field: SerializeField] public float KnockbackPower { get; set; }
    [field: SerializeField] public bool CanJump { get; set; }
    [field: SerializeField] public int Gauge { get; set; }
    public bool isMoved = true;
    private Dictionary<StatType, float> stats = new();

    private void Awake()
    {
        InitStats();
    }

    public void InitStats()
    {
        CharacterManager.Instance.PlayerStat = this;

        maxHealth = 5;
        CurrentHealth = 5;

        stats.Add(StatType.Attack, Attack); 
        stats.Add(StatType.Health, CurrentHealth);
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
        Debug.Log("������ ����");
        Debug.Log($"���� ü��: {CurrentHealth}");
        CurrentHealth -= 1;
        Debug.Log($"�ٲ� ü��: {CurrentHealth}");

        if (CurrentHealth <= 0)
            CurrentHealth = 0;
    }
}
