# A Untitled Unity Game

A month ago I started making a roquelike game in Unity using C#. And since I have two years of coding experience in C, I decided to make this project a big one.

![GameScreenshot](https://github.com/steryu/UnityGamey/blob/main/images/Screenshot%202023-11-17%20110857.png)

Here are a couple of code snippets to show how I implement features. There are probaly better ways to do this, but for now this is what i came up with :).
## Snippets of Code Structures
- [Ability System](#ability-system)<br>
- [Enemy Spawner](#enemy-spawn-system)<br>
- [EXP Absorber](#exp-absorber)<br>
- [Future Implements](#future-implements)<br>
- [Disclaimer](#disclaimer)<br>

#### Ability System
RoqueLike games are my favorite genres and what I enjoy most about it, is the extensive and diverse range of abilities that can be upgraded and merged together with other ones. I hope to do the same in my project. Using a good data managment system to easily add abilities on the fly was tricky to do, especially since I'm doing this on my own. But I've implemented a way, that I believe is handy.

I started with the `Ability Base class`. It holds all the basic data an ability needs. Then I added an `AbilityUpgrade class` for upgrades, and put it into an array within the base class.

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
The `UniqueAbility class` inherits from the `base class` and extends functionality to make it possible to add unique properties, such as elemental damage. This class is implemented as a ScriptableObject, providing a interface for tweaking properties directly within the Unity inspector. With this approach I can create an abilty with an array of upgrades like this:

![UpgradesInspector](https://github.com/steryu/UnityGamey/blob/main/images/Screenshot%202023-11-17%20140828.png)

To manage the available abilities, I made a database. Every time the player gains experience points and levels up, they are presented with a selection of three abilities and these choices are randomly drawn from the ability database.

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
When an ability is chosen, a duplicate is instantiated in the "arsenal," to prevent overwriting the base values. Simultaneously, a copy of the corresponding upgrade is made and added into the database, replacing the previous version. 

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
The abilities stored in the arsenal are automaticly called in the script's `update()` method.

#### Enemy spawn system
I wanted to make an enemy spawning system that spawns enemies with an defined interval and amount. To achieve this, I created a `Spawner class`, that holds an array of `EnemySpawner` ScriptableObject instances.

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

The `spawner class` will first initialize a timer that is used for the interval between spawns. In the `update()` method I loop trough the array of `EnemySpawner` instances. This loop calls the spawning of enemies based on the predefined intervals and instatiates the enemies within a radom customizable range. They then automaticly follow the player.

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

#### EXP absorber
To implement a EXP absorption mechanic in my game, I crafted a function called CollectAllEXPOrbs. This function locates all game objects tagged as 'Exp' within the scene. I then calculated the direction of each EXP orb in relation to the player and moved them toward the player's position using `Math.Min()`, creating a visually satisfying magnet effect. <br>
However, an issue I have now is that when an enemy is destroyed and the EXP orb is instantiated, that orb will also move towards the player when the `EXP magnet` is active. To fix this I probably need to create a list that takes all the **current** orbs in scenes and loop trough that instead of relying on a fixed amount.

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
				}
				if (expOrb != null)
				{
					Destroy(expOrb.gameObject);
				}
			}
		}
	}
}
```

I hope these little snippets showcases my proficiency in implement game mechanics.

#### Future Implements
I'm having so much fun coding this game. There are a lot of challanges but it's so satisfiyting when you figure it out and it works, and when you see it in game it makes all the stuggles worth it. I've got lots and lots of ideas, and I'm committed to putting in the time and effort to bring them to life. I'm so excited!

I'll try to make the game minimal, but here are some key features in mind:
- Loads of abilities and more of enemies
- To be able to merge skills for unique variations to keep every run fresh
- A solid scaling system
- A rich storyline with an affinity system to unlock new abilities
- A touch of cooking
  and more...

#### Disclaimer
This project is still a work in progress. It's public only so I can showcase it in my portfolio.
