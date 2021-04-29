using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationGrid : MonoBehaviour
{
	[Header("[Grid Information]")]
	[SerializeField] private Vector2 gridSize = new Vector2(6, 6);
	[SerializeField] private float spaceBetween = 5;
	[SerializeField] private float spaceVariation = 2f;
	[SerializeField] private GameObject[] prefabs;
	
    void Start()
    {
        for (int line = 0; line < gridSize.x; line ++)
		{
			for (int column = 0; column < gridSize.y; column ++)
			{
				Vector3 position = new Vector3(line * spaceBetween + Random.Range(-spaceVariation, spaceVariation), -1f, column * spaceBetween + Random.Range(-spaceVariation, spaceVariation));
				Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.Euler(0, Random.Range(-180, 180), 0));
			}
		}
    }
}
