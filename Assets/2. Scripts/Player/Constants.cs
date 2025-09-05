using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour 
{
    public static class ColorAlpha
    {
        public const float ZERO = 0.0f;
        public const float ONE = 1.0f;
        public const float HALF = 0.5f;
    }

    public static class LayerName
    {
        public const int DEFAULT = 0;
        public const int INVINCIBLE = 7;
    }

    public static class CountValue
    {
        public const float HIT = 1.5f; // ������ �ð�
        public const float INVINCIBLE_BLINK_TIME = 0.1f; // ������ �ð� ����
    }

    public static class AnimationParameter
    {
        public const string MOVE = "Move";
        public const string RUN = "Run";
        public const string HIT = "Hit";
        public const string ATTACK = "Attack";
        public const string SPECIALATTACK = "SpecialAttack";
        public const string CLIMB = "Climb";
    }

    public static class State
    {
        public const string IDLE = "PlayerIdleState";
        public const string MOVE = "PlayerMoveState";
        public const string RUN = "PlayerRunState";
        public const string JUMP = "PlayerJumpState";
        public const string HIT = "PlayerHitState";
    }

    public static class CoolTime
    {
        //public const float ATTACK = 0.5f;
        public const float ATTACK = 1.05f;
    }

    public static class SpecialAttack
    {
        public const int GUAGE = 5;
    }
}
