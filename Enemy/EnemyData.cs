using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
	[SerializeField] private GameObject _expPrefab;
    public MeshRenderer meshRenderer;
    public GameObject FloatingTextPrefab;
	public GameObject VFX_damage;
	private GameObject _currentVFX;

	private int _kills = 0;

	// enemy variables
	public float enemySpeed;
	public float Damage;
    [SerializeField] private float health;
    [SerializeField] private float currentHealth;

	// flash color
	[SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private Color originalColor;
	[SerializeField] private Color FlashColor;

    private void Start()
	{
        originalColor = meshRenderer.material.color; 
        currentHealth = health;
    }
	void Update()
	{

	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Witch"))
		{
			other.GetComponent<PlayerHealth>().TakeDamage(Damage);
		}
	}

	public void TakeDamage(float Damage)
	{
		currentHealth -= Damage;

		// Floating damage text
		if (FloatingTextPrefab)
			ShowFloatingText(Damage);

		// change material
		StartCoroutine(Flash());

		if (currentHealth <= 0)
		{
			Die();

			_kills = PlayerPrefs.GetInt("Kills");
			_kills++;
			PlayerPrefs.SetInt("Kills", _kills);
		}
	}

	IEnumerator Flash()
	{
        // Create an array to store all the renderers in the game object and its children
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        // Apply the flash color to all materials in all renderers
        foreach (Renderer renderer in allRenderers)
        {
			renderer.material.color = FlashColor;
        }

        yield return new WaitForSeconds(_flashDuration);

        // Revert the color for all materials in all renderers
        foreach (Renderer renderer in allRenderers)
        {
            renderer.material.color = originalColor;
        }
    }

	void ShowFloatingText(float Damage)
	{
		Vector3 offset = new Vector3(Random.Range(-0.4f, 0.4f), 0f, 0f);
		var text = Instantiate(FloatingTextPrefab, transform.position + offset, Quaternion.identity, transform);

		var floatingTextObject = new GameObject("FloatingText");
		floatingTextObject.transform.position = text.transform.position;

		text.transform.SetParent(floatingTextObject.transform, worldPositionStays: false);

		text.GetComponent<TextMeshPro>().text = Damage.ToString();
	}

	public void Die()
	{
		if (VFX_damage != null)
		{
			if (_currentVFX != null)
			{
				Destroy(_currentVFX);
			}
			_currentVFX = Instantiate(VFX_damage, transform.position, Quaternion.Euler(90, 0, 0));
		}
		Destroy(gameObject);
		transform.position = new Vector3(transform.position.x, _expPrefab.transform.position.y, transform.position.z);
		Instantiate(_expPrefab, transform.position, Quaternion.identity);
	}

}
