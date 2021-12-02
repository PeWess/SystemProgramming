using System.Collections.Generic;
using SolarSystemKritskiy_Main;
using SolarSystemKritskiy_Mechanics;
using UnityEngine;
using UnityEngine.Networking;

public class ServiceLocator : Singleton<ServiceLocator>
{
    public string PlayerNickname { get; private set; }
    public CameraOrbit CameraOrbit { get; private set; }
    public Transform[] SpawnPoints { get; private set; }
    public List<Vector3> CrystalSpawns { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    
    public void SetNickname(string nickname)
    {
        PlayerNickname = nickname;
    }
    
    public void BindCameraOrbit(CameraOrbit orbit)
    {
        CameraOrbit = orbit;
    }

    public void SetSpawnPoints(Transform[] spawnPoints)
    {
        SpawnPoints = new Transform[spawnPoints.Length - 1];
        for (int i = 1; i < spawnPoints.Length; i++)
        {
            SpawnPoints[i - 1] = spawnPoints[i];
        }
    }

    public void SetCrystals(List<Vector3> crystalSpawns)
    {
        CrystalSpawns = crystalSpawns;
    }

    public void UnbindCameraOrbit()
    {
        CameraOrbit = null;
    }
}
