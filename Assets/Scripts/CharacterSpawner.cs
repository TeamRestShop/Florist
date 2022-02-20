using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterSpawner : MonoBehaviour
{
    [Serializable]
    private struct RandomPositionRange
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
    }

    [SerializeField] private RandomPositionRange spawnArea;
    [SerializeField] private GameObject characterPrefab;

    private void Start()
    {
        var spawnPosition = new Vector2(Random.Range(spawnArea.MinX, spawnArea.MaxX),
            Random.Range(spawnArea.MinY, spawnArea.MaxY));

        PhotonNetwork.Instantiate($"Prefab/{characterPrefab.name}", spawnPosition, Quaternion.identity);
    }
}