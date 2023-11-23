using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    [SerializeField] private Rect spawnArea = new Rect(-5f, -5f, 10f, 10f);

    private void OnDrawGizmos()
    {
        // Рисуем границы области спавна в сцене для визуализации
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
    private void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax), Random.Range(spawnArea.yMin, spawnArea.yMax));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }


}
