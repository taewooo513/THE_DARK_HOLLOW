using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Attack,
    MaxHealth,
    Health,
    MoveSpeed,
    SpeedModifier,
    SpeedModifierInput,
    JumpPower,
    Gauge
}

public class PlayerStat : MonoBehaviour
{
    [field: SerializeField] public int Attack { get; set; }
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

    public Vector2 playerCheckPoint;

    private void Awake()
    {
        InitStats(CharacterManager.Instance.playerData);
    }

    public void InitStats(PlayerData data)
    {

        CharacterManager.Instance.PlayerStat = this;
        maxHealth = 5;
        if (data == null)
        {
            data = new PlayerData();
            CurrentHealth = 5;
            data.hp = CurrentHealth;
        }
        else
        {
            CurrentHealth = data.hp;
        }
        CurrentHealth = 5;
        MoveSpeed = 2;
        JumpPower = 6;


        stats.Add(StatType.Attack, Attack);
        stats.Add(StatType.MaxHealth, maxHealth);
        stats.Add(StatType.Health, CurrentHealth);
        stats.Add(StatType.MoveSpeed, MoveSpeed);
        stats.Add(StatType.SpeedModifier, SpeedModifier);
        stats.Add(StatType.SpeedModifierInput, SpeedModifierInput);
        stats.Add(StatType.JumpPower, JumpPower);
        stats.Add(StatType.Gauge, Gauge);
    }

    public void ReInitStats(float b)
    {
        CharacterManager.Instance.PlayerStat = this;

        CurrentHealth = b;

        stats.Add(StatType.Attack, Attack);
        stats.Add(StatType.MaxHealth, maxHealth);
        stats.Add(StatType.Health, CurrentHealth);
        stats.Add(StatType.MoveSpeed, MoveSpeed);
        stats.Add(StatType.SpeedModifier, SpeedModifier);
        stats.Add(StatType.SpeedModifierInput, SpeedModifierInput);
        stats.Add(StatType.JumpPower, JumpPower);
        stats.Add(StatType.Gauge, Gauge);
    }

    public float SetHealth(float b)
    {
        b = CurrentHealth;

        return b;
    }

    public float GetStat(StatType statType)
    {
        return stats[statType];
    }

    public void TakeDamage()
    {
        Debug.Log("데미지 입음");
        Debug.Log($"기존 체력: {CurrentHealth}");
        CurrentHealth -= 1;
        Debug.Log($"바뀐 체력: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            SaveData saveData = CharacterManager.Instance.saveData;
            if (saveData == null)
            {
                if (CharacterManager.Instance.playerData != null)
                {
                    CharacterManager.Instance.playerData.hp = 5;
                }
                SceneLoadManager.Instance.LoadScene(SceneKey.stage1Scene);
            }
            else
            {
                if (CharacterManager.Instance.playerData != null)
                {
                    CharacterManager.Instance.playerData.hp = saveData.hp;
                }
                else
                {
                    CharacterManager.Instance.playerData = new PlayerData();
                    CharacterManager.Instance.playerData.hp = saveData.hp;
                }
                SceneLoadManager.Instance.LoadScene(saveData.sceneKey);
            }
            CurrentHealth = 0;
        }
    }

    public Dictionary<StatType, float> GetPlayerStat()
    {
        return stats;
    }

    public void SetCheckPoint()
    {
        TakeDamage();
        transform.position = playerCheckPoint;
    }
}
