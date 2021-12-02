using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject _crystalPrefab;
    [SyncVar] private Transform[] _spawners;
    [SyncVar] private List<Vector3> _crystalSpawners;

    private void Start()
    {
        _spawners = GetComponentsInChildren<Transform>();
        ServiceLocator.Instance.SetSpawnPoints(_spawners);

        _crystalSpawners = SetCrystalSpawners();
        ServiceLocator.Instance.SetCrystals(_crystalSpawners);
        SpawnCrystals();
    }

    private List<Vector3> SetCrystalSpawners()
    {
        List<Vector3> crystals = new List<Vector3>();
        Random rnd = new Random();
        int posX;
        int posY;
        int posZ;

        for (int i = 0; i < 15; i++)
        {
            posX = 0;
            posY = 0;
            posZ = 0;
            while (posX >= -250 && posX <= 250 && posY >= -250 && posY <= 250 && posZ >= -250 && posZ <= 250)
            {
                posX = rnd.Next(-2000, 2001);
                posY = rnd.Next(-500, 501);
                posZ = rnd.Next(-2000, 2001);
            } 
            crystals.Add(new Vector3(posX, posY, posZ));
        }
        
        return crystals;
    }
    
    public void SpawnCrystals()
    {
        GameObject crystal;
        foreach (var point in ServiceLocator.Instance.CrystalSpawns)
        {
            crystal = Instantiate(_crystalPrefab);
            crystal.transform.position = point;
            NetworkServer.Spawn(crystal);
        }
    }
}
