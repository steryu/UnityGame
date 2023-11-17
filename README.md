# A Untitled Unty game

A month ago I started making a roquelike game in Unity using C#. I always wanted to start a hobby project like this and since I have two year coding experience, I decided to make my first project a bigger one. Learning C# was also not difficult at all because my first language was C.

![GameScreenshot](https://github.com/steryu/UnityGamey/blob/main/images/Screenshot%202023-11-17%20110857.png)

Here are a couple of code snippets to show how I implement features. There are probaly better ways to do this, but for now this is what i came up with :).
## Snippets of Code Structures
[Ability System](#ability-system)<br>
[Enemy Spawner](#ability-system)<br>
[EXP aborber](#ability-system)<br>
[UI](#ability-system)<br>
#### Ability System
RoqueLike games are my favorite genres and what I enjoy most about it, is the extensive library of abilities that can be upgraded an merge with other ones. I hope to do the same in my project. Using a good data managment system to easily add abilities on the fly was tricky to do, especially since i'm doing this on my own. But i've implement a way, that i believe is handy.

I made a `ability base class` with all the data a basic ability needs and `abilityUpgrade class` for upgrades. 
The upgrades are added as an array in the base class.

```C#
public class Ability : ScriptableObject
{
    public GameObject Prefab;
    public string BaseName;
    public string Name;
    public string Description;
    public int Level;

    public SubStats subStats;
    [System.Serializable]
    public class SubStats
    {
        public float Damage;
        public float ActiveTime;
        public float CooldownTime;
    }
    // other base abilities...
    public AbilityUpgrade[] Upgrades;
}

[System.Serializable]
public class AbilityUpgrade
{
    public string NewName;
    public string NewDescription;
    public float DamageAdd;
    public float DamageMult;
    public int Level;
    public int Amount;
}
```
After that i made `unique ability class` with the possibility to have unique properties, like elemental damage, that inherits form the `base ability class`. This class is also scriptable object which means i can easily adjust properties in the inspector. with this system I can create an abilty with an array of upgrades like this:

![UpgradesInspector](https://github.com/steryu/UnityGamey/blob/main/images/Screenshot%202023-11-17%20140828.png)

Then I put the abilties that are unlocked to the player in a database. Every time the player levels up in game by collecting EXP points you have the options the select 1 of 3 abilities. These are abilties chosen at random from the `database`. 
```C#
private void DisplayAbilities()
{
    // 3 random abilities
    List<Ability> abilities = database.GetAbilities();
    if (abilities == null)
    {
        Debug.Log("error getting receiving abilities list");
    }
    if (abilities.Count > 0
    {
        foreach (var abilityButton in _abilityButton)
        {
            var ability = abilities[UnityEngine.Random.Range(0, abilities.Count)];
            abilityButton.Init(ability.Name, ability.Description);
            abilityButton.Button.onClick.AddListener(() =>
            {
                if (ability.isPassive)
                    abilityManager.setPassiveUpgrade(ability);
                else
                    abilityManager.AddAbilityToArsenal(ability);
                  SetInactive();
            });
        }
        // error management
    }
}
```

When the ability is chosen, a copy is is put in the `arsenal` to prevent overwriting the base values and at the same time a copy of the upgrade for that ability is made an put in the database while the old one is removed. 

```C#
public void AddAbilityToArsenal(Ability ability)
{
    RemovePreviousUpgrade(ability);

    Ability newAbility = Instantiate(ability);
    arsenal.Add(newAbility);

    GameObject instantiatedObject = Instantiate(newAbility.Prefab, playerTransform);
    newAbility.SetInstantiatedObject(instantiatedObject);

    if (ability.Level < ability.Upgrades.Length)
    {
        Ability UpgradedAbility = Instantiate(ability);
        AbilityUpgrade upgrade = ability.Upgrades[ability.Level];
        UpgradedAbility.ApplyUpgrade(upgrade);

        m_Database.addAbility(UpgradedAbility);
        m_Database.RemoveAbility(ability.Name);
    }
    if (ability.Level == ability.Upgrades.Length)
    {
        m_Database.RemoveAbility(ability.Name);
        Debug.Log("Removed: " + ability.Name);
    }
}
```
The abilities in the arsenal are automaticly called in update().


#### Enemy spawn system



#### EXP aborsber

#### UI

This project is still a work in progress. It's public only so I can show it in my portfolio
