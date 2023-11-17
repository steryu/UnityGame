using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic orb", menuName = "New Ability/Magic orb")]
public class MagicOrbAbility : Ability
{
    protected override void ApplyUpgradeEffect(AbilityUpgrade upgrade)
    {
        if (upgrade is AbilityUpgrade magicOrbUpgrade)
        {
            Name = magicOrbUpgrade.NewName;
            Description = magicOrbUpgrade.NewDescription;
            subStats.Damage += magicOrbUpgrade.DamageAdd;
            Level = magicOrbUpgrade.Level;
            subStats.Amount = magicOrbUpgrade.Amount;
        }
    }
}
