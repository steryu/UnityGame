using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ember Ball", menuName = "New Ability/Ember Ball")]
public class EmberBallUpgrade : Ability
{
    protected override void ApplyUpgradeEffect(AbilityUpgrade upgrade)
    {
        if (upgrade is AbilityUpgrade EmberBall)
        {
            Name = EmberBall.NewName;
            Description = EmberBall.NewDescription;
            subStats.Damage += EmberBall.DamageAdd;
            Level = EmberBall.Level;
        }
    }
}
