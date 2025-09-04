using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseState : BossStateBase
{
    public ChooseState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        if (!ctx.CanSeePlayer()) 
        { 
            fsm.Change(ctx.SIdle); 
            return; 
        }
        float d = ctx.Dist;
        var s = ctx.stat;

        //===============[유틸리티 계산]===============
        // 새로운 공격 패턴이 생길때 마다 여기에 추가
        // 거리 → 가우시안 점수
        float sigmaNear = Mathf.Max(0.01f, s.nearRange * s.nearSigmaFrac);
        float sigmaMid = Mathf.Max(0.01f, s.nearRange * s.nearSigmaFrac);
        float sigmaFar = Mathf.Max(0.01f, s.farRange * s.farSigmaFrac);
        // 가우시안 점수 * 쿨타임 가능 여부(0/1) * 가중치
        float scoreDash = Gauss(d, s.nearRange, sigmaNear) * (ctx.CDReadyDash() ? 1f : 0f) * s.weightDash;
        float scoreMid = Gauss(d, s.nearRange, sigmaNear) * (ctx.CDReadyDash() ? 1f : 0f) * s.weightDash;
        float scoreRng = Gauss(d, s.farRange, sigmaFar) * (ctx.CDReadyRanged() ? 1f : 0f) * s.weightRanged;

        // 이전에 선택된 공격 패턴이 다시 선택될 확률 증가
        if (ctx.lastChosen == BossController.AttackChoice.Dash) scoreDash *= (1f + s.stickiness);
        if (ctx.lastChosen == BossController.AttackChoice.Mid) scoreDash *= (1f + s.stickiness);
        if (ctx.lastChosen == BossController.AttackChoice.Ranged) scoreRng *= (1f + s.stickiness);

        // 미세한 무작위성 추가로 겹침 방지
        scoreDash += Random.Range(-s.utilityNoise, s.utilityNoise);
        scoreMid += Random.Range(-s.utilityNoise, s.utilityNoise);
        scoreRng += Random.Range(-s.utilityNoise, s.utilityNoise);

        float best = Mathf.Max(scoreDash, scoreRng, scoreMid);
        //===============[유틸리티 계산]===============

        // 실행할 만큼 점수가 낮으면 거리 조정로 복귀
        if (best < s.minScoreToAct)
        {
            fsm.Change(ctx.SRecover); 
            return;
        }
        // 7) 전이 + 마지막 선택 저장
        if (best == scoreDash)
        {
            ctx.lastChosen = BossController.AttackChoice.Dash;
            fsm.Change(ctx.SAtkDash);
        }
        else if (best == scoreRng)
        {
            ctx.lastChosen = BossController.AttackChoice.Ranged;
            fsm.Change(ctx.SAtkRng);
        }
        else if (best == scoreMid)
        {
            ctx.lastChosen = BossController.AttackChoice.Mid;
            fsm.Change(ctx.SAtkMid);
        }

    }
    // 가우시안 유틸리티 함수: μ 중심, σ 폭
    // x가 μ에 가까울수록 1에 가까운 점수, 멀어질수록 0에 가까운 점수
    static float Gauss(float x, float mu, float sigma)
    {
        if (sigma <= 1e-4f) return (Mathf.Abs(x - mu) <= 1e-4f) ? 1f : 0f;
        float z = (x - mu) / sigma;
        return Mathf.Exp(-0.5f * z * z);
    }
}
