using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnHeals : MonoBehaviour
{
    public GameObject healPrefab;
    public int numberOfHeals = 15; // Количество объектов heal для спауна
    [SerializeField] private Rect spawnArea = new Rect(-5f, -5f, 10f, 10f);

    private void Start()
    {
        // Проверяем, является ли текущий игрок первым (мастер-клиентом)
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            SpawnHealsInRandomArea();
        }
    }

    private void OnDrawGizmos()
    {
        // Рисуем границы области спавна в сцене для визуализации
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }

    private void SpawnHealsInRandomArea()
    {
        for (int i = 0; i < numberOfHeals; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax));
            PhotonNetwork.Instantiate(healPrefab.name, randomPosition, Quaternion.identity);
        }
    }
}
