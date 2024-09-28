using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpamChars : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // Spawn noktalarý
    public List<GameObject> prefabs; // Karakter prefab'larý listesi

    private void Start()
    {
        if (IsServer)
        {
            SpawnCharacters();
        }
    }

    private void SpawnCharacters()
    {
        // Harvester'ýn spawn edilip edilmeyeceðini kontrol et
        bool hasSpawnedHarvester = false;

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {
            var clientId = NetworkManager.Singleton.ConnectedClientsIds[i];
            var spawnPoint = spawnPoints[i % spawnPoints.Length];

            GameObject characterPrefab;
            characterPrefab = prefabs[i];
            /*if (!hasSpawnedHarvester && i == 0)
            {
                characterPrefab = prefabs[0]; // 0: Harvester
                hasSpawnedHarvester = true; // Harvester'ý spawn ettik
            }
            else
            {
                
            }*/

            // Karakteri spawn et
            GameObject character = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
            character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // NetworkObject'i client ile iliþkilendir
        }
    }
}
