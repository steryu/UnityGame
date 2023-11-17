using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpAbsorber : MonoBehaviour
{
    private PlayerExp _playerExp;
    public float attractionRadius = 2f;
    public float attractionSpeed = 5f;

    private void Start()
    {
        _playerExp = GameObject.Find("Witch").GetComponent<PlayerExp>();
    }

    private void Update()
    {
        AbsorbNearbyExp();
    }

    private void AbsorbNearbyExp()
    {

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attractionRadius);

        foreach (Collider collider in nearbyColliders)
        {
            if (collider.CompareTag("Exp"))
            {
                PickupItem exp = collider.GetComponent<PickupItem>();

                // Calculate the direction from the player to the exp orb
                Vector3 direction = collider.transform.position - transform.position;

                // Calculate the distance between the player and the exp orb
                float distance = direction.magnitude;

                // If the player is close enough, absorb the exp orb
                float t = Mathf.Clamp01(attractionSpeed * Time.deltaTime / distance);

                // Interpolate the position towards the player
                Vector3 newPosition = Vector3.Lerp(collider.transform.position, transform.position, t);

                // Update the position of the exp orb
                collider.transform.position = newPosition;

                // If the orb is close enough, absorb it
                if (distance < 0.1f)
                {
                    _playerExp.setExp(exp.expValue);
                    Destroy(collider.gameObject);
                }
            }
        }
    }
}
