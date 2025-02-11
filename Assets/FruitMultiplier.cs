using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitMultiplier : MonoBehaviour
{
    public LayerMask fruitLayer; // Layer des fruits
    public int multiplicationFactor = 2; // Nombre de fois qu'un fruit est dupliqué
    public float cooldown = 2f; // Temps d'attente avant qu'un fruit puisse être à nouveau multiplié

    private HashSet<GameObject> cooldownFruits = new HashSet<GameObject>(); // Liste des fruits en cooldown

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & fruitLayer) != 0 && !cooldownFruits.Contains(other.gameObject))
        {
            StartCoroutine(MultiplyFruit(other.gameObject));
        }
    }

    private IEnumerator MultiplyFruit(GameObject fruit)
    {
        cooldownFruits.Add(fruit); // Ajoute le fruit en cooldown

        for (int i = 0; i < multiplicationFactor; i++)
        {
            GameObject newFruit = Instantiate(fruit, fruit.transform.position, Quaternion.identity);
            newFruit.transform.position += new Vector3(0.5f * (i + 1), 0, 0);
        }

        yield return new WaitForSeconds(cooldown); // Attente du cooldown

        cooldownFruits.Remove(fruit); // Retire le fruit du cooldown
    }
}


