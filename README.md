# A Untitled Unty game

A month ago I started making a roquelike game in Unity using C#. I always wanted to start a hobby project like this and since I have two year coding experience, I decided to make my first project a bigger one. Learning C# was also not difficult at all because my first language was C.

![GameScreenshot](https://github.com/steryu/UnityGamey/blob/main/images/Screenshot%202023-11-17%20110857.png)

Here are a couple of code snippets to show how I implement features. There are probaly better ways to do this, but for now this is what i came up with :).
## Snippets of Code Structures
[Ability System](#ability-system)<br>
[Enemy Spawner](#Enemy-spawn-system)<br>
[EXP aborber](#EXP-aborsber)<br>
[Future Implements](#Future-Implements)<br>
[Disclaimer](#Disclaimer)<br>

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
I wanted to make a spawn system that spawn emeies with an interval and an certain amount. I did this by creating an `spawner class` that hold an array of `EnemySpawner scriptible objects`

```C#
[CreateAssetMenu(fileName = "EnemySpawner", menuName = "Enemy Spawner/Enemy Type")]
public class EnemySpawner : ScriptableObject
{
    public GameObject prefab;
    public float spawnTime;
    public Vector2 spawnRange;
    public float spawnInterval;
    public int amount;
}
```

The `spawner class` wil first intiziale a timer that use for the interval and the in de update() method loop trough the `EnemySpawer` array and spawn the enemies within a radom customizeable range and then automaticly follow the player.

```C#
public class Spawner : MonoBehaviour
{
    Transform _playerPostion;
    [SerializeField] private float _initialDelay = 1f;
    private float _currentangle;
    private bool _isActive;
    [SerializeField] Timer timer;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public EnemySpawner[] enemySpawner;

    private List<float> spawnTimers = new List<float>();

    private IEnumerator Start()
    {
        _playerPostion = GameObject.Find("Player").GetComponent<Transform>();
        foreach (var spawner in enemySpawner)
        {
            spawnTimers.Add(spawner.spawnInterval);
        }
        yield return new WaitForSeconds(_initialDelay);
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        for (int i = 0; i < enemySpawner.Length; i++)
        {
            var e = enemySpawner[i];
            spawnTimers[i] += Time.deltaTime;

            if (timer.elapsedTime > e.spawnTime && spawnTimers[i] >= e.spawnInterval)
            {
                for (int j = 0; j < e.amount; j++)
                {
                    GameObject spawnedEnemy = Spawn(e);
                    if (spawnedEnemy != null)
                    {
                        spawnedEnemies.Add(spawnedEnemy);
                    }
                }
                spawnTimers[i] = 0f;
            }
        }
    }

    GameObject Spawn(EnemySpawner e)
    {
        _currentangle += 180f + Random.Range(-45, 45);
        var angleInRad = _currentangle * Mathf.Rad2Deg;
        var range = Random.Range(e.spawnRange.x, e.spawnRange.y);
        var relativePosition = new Vector3(Mathf.Cos(angleInRad) * range, e.prefab.transform.position.y, Mathf.Sin(angleInRad) * range);
        var spawnPosition = _playerPostion.position + relativePosition;

        GameObject spawnedEnemy = Instantiate(e.prefab, spawnPosition, Quaternion.identity, transform);
        return spawnedEnemy;
    }
```

#### EXP aborsber
When the player pick's up and `EXP manget` all the EXP orbs in the scene should move to the player and be "aborbed". To create this a made an function called `collectallEXPorbs` in this function if look for all the gameobject using the tag 'Exp' then i calculaste the direction of the orb from the player and move it towards it using `Math.Min()`. Then i destroy the gameObjects. I have an issue that when an enemy is destroyed and drops and EXP orb that orb will also move towards the player when the `exp magnet` is active. To fix this I probably need to make an list that takes all the current orbs in scenes al loop trough that instead of realying on a fixed amount.

```C#
private void CollectAllExpOrbs()
{
	List<GameObject> expOrbsToDestroy = new List<GameObject>();
	GameObject[] expOrbs = GameObject.FindGameObjectsWithTag(_expOrbTag);

	_amount = expOrbs.Length;
	foreach (GameObject expOrb in expOrbs)
	{
		if (expOrb.CompareTag("Exp"))
		{
			PickupItem exp = expOrb.GetComponent<PickupItem>();

			Vector3 position = new Vector3(_player.position.x, 1f, _player.position.z);
			Vector3 direction = position - expOrb.transform.position;

			float distance = direction.magnitude;
			direction.Normalize();

			expOrb.transform.position += direction * Mathf.Min(_attractionSpeed * Time.deltaTime, distance);

			if (distance < 0.1f)
			{
				if (exp != null)
				{
					_amount--;
					_playerExp.setExp(exp.expValue);
					if (expOrb != null)
					{
						Destroy(expOrb.gameObject);
					}
                }
            }
        }
	}
}
```

## Future Implements
These were code snippets of some of the many scripts i made to create these features. I plan to use code only to make my dream game. I'm having so much fun creating this game and i have some many ideas to want to bring to live! It will time to do it but ill do my best to make it come trough.

Ill try to make the game "simple" and minimal, but here are some features that i want to implement:
- Lots of abilities and a couple of enemioes
- Able to merge abilies to make unique variations to make every run different
- Rich story line and driven with an affinity system to unlock new abilities
- Cooking
- and more...

#### Disclaimer
This project is still a work in progress. It's public only so I can show it in my portfolio
