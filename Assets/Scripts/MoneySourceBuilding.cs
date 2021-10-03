using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySourceBuilding : Building
{
    public float MoneyEarnTimer = 10f;
    float MoneyEarnTimerLeft = 10f;
    public float MoneyEarnAmount = 10f;
    public float MoneyEarnMultiplierPerLevel = 1f;
    public ParticleSystem MoneyEarnParticle;
    private void Update()
    {
        if (MoneyEarnTimerLeft > 0)
            MoneyEarnTimerLeft -= 1f * Time.deltaTime;
        else
        {
            EarnMoney();
        }
    }

    public void EarnMoney()
    {
        if(MoneyEarnParticle)
            MoneyEarnParticle.Play();
        MoneyEarnTimerLeft = MoneyEarnTimer;
        float moneyEarnAmount = MoneyEarnAmount * (MoneyEarnMultiplierPerLevel * (Level + 1));
        DataManager.Instance.MoneyIncrease((int)moneyEarnAmount);
    }
}
