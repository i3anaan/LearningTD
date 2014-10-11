using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DeserializedLevelLoader
{
    public static void updateWaveSpawner()
    {
        WaveSpawner existingWS = GameObject.FindObjectOfType<WaveSpawner>();
        DeserializedWaveSpawner dws = XmlIO.LoadXml<DeserializedWaveSpawner>("WaveSpawner");

        existingWS.timeBetweenWaves = int.Parse(dws.timeBetweenWaves);
        existingWS.maxTimePerWave = int.Parse(dws.maxTimePerWave);
        existingWS.endless = bool.Parse(dws.endless);
        existingWS.wave = int.Parse(dws.wave);
        existingWS.difficulty = int.Parse(dws.difficulty);
        existingWS.startPosition = dws.startPosition.toVector3();
        existingWS.targetPosition = dws.targetPosition.toVector3();
    }
}