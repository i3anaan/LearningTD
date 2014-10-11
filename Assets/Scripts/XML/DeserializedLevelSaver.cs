using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DeserializedLevelSaver
{
    public static string prefabsFolder = "Prefabs/Waves/";


    public static void saveWaveSpawner(WaveSpawner ws)
    {
        DeserializedWaveSpawner dws = new DeserializedWaveSpawner();
        dws.timeBetweenWaves = ws.timeBetweenWaves.ToString();
        dws.maxTimePerWave = ws.maxTimePerWave.ToString();
        dws.endless = ws.endless.ToString();
        dws.wave = ws.wave.ToString();
        dws.difficulty = ws.difficulty.ToString();
        dws.startPosition = DeserializedWaveSpawner.getOverwritingSVector3(ws.startPosition);
        dws.targetPosition = DeserializedWaveSpawner.getOverwritingSVector3(ws.targetPosition);

        AbstractWave[] existingWaves = ws.GetComponentsInChildren<AbstractWave>();
        DeserializedWaveSpawner.DeserializedWave[] deserializedWaves = new DeserializedWaveSpawner.DeserializedWave[existingWaves.Length];



        Dictionary<string, AbstractWave> wavePrefabPool = new Dictionary<string, AbstractWave>();

        for (int i = 0; i < existingWaves.Length;i++)
        {
            DeserializedWaveSpawner.DeserializedWave wave = new DeserializedWaveSpawner.DeserializedWave();
            AbstractWave ewave = existingWaves[i];

            


            wave.prefab = ewave.name;

            //Load prefab into pool (to be able to compare values)
            if (!wavePrefabPool.ContainsKey(wave.prefab))
            {
                // load prefab
                AbstractWave prefabObject = Resources.Load(prefabsFolder + wave.prefab, typeof(AbstractWave)) as AbstractWave;

                // if unsuccesful, error message and jump to next in the foreach loop
                if (prefabObject == null)
                {
                    Debug.LogError("Prefab \"" + wave.prefab + "\" does not exists.");
                    continue;
                }

                // otherwise add to dictionary
                wavePrefabPool.Add(wave.prefab, prefabObject);
            }


            AbstractWave prefab = wavePrefabPool[wave.prefab];
            wave.startPosition = DeserializedWaveSpawner.getOverwritingSVector3(prefab.startPosition,ewave.startPosition);
            wave.amountPerType = ewave.amountPerType.Select(x => x.ToString()).ToArray();
            //TODO wave.amountPerType null when equal
            wave.difficulty = DeserializedWaveSpawner.toStringNullWhenEqual(prefab.difficulty,ewave.difficulty);
            wave.creepStupidity = DeserializedWaveSpawner.toStringNullWhenEqual(prefab.creepStupidity,ewave.creepStupidity);

            deserializedWaves[i] = wave;
        }

        dws.waves = deserializedWaves;

        XmlIO.SaveXml<DeserializedWaveSpawner>(dws, "./Assets/Resources/" + DeserializedWaveSpawner.xmlExportName + ".xml");
    }
}
