using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float hp;
    Vector3 lastCheckPoint;
}

public class CharacterManager : Singleton<CharacterManager>
{
    public PlayerData playerData; // 플레이어가 씬전환시 유지되어야 하는 데이터
    // Player, 몬스터 등 캐릭터 관리
    public Boss _boss;
    public Boss Boss { get { return _boss; } set { _boss = value; } }

    // Player
    public PlayerStat _playerStat;
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }


    public OnTriggerCamera _onTriggerCamera;
    public OnTriggerCamera OnTriggerCamera { get { return _onTriggerCamera; } set { _onTriggerCamera = value; } }


}
