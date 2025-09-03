using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    // Player, 몬스터 등 캐릭터 관리
    public Boss _boss;
    public Boss Boss { get { return _boss; } set { _boss = value; } }

    // Player
    public PlayerStat _playerStat;
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }
}
