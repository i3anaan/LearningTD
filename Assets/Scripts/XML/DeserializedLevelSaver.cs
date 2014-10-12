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
        dws.startWave = ws.startWave.ToString();
        dws.difficulty = ws.difficulty.ToString();
        dws.startPosition = DeserializedWaveSpawner.getOverwritingSVector3(ws.startPosition);
        dws.targetPosition = DeserializedWaveSpawner.getOverwritingSVector3(ws.targetPosition);

        AbstractWave[] existingWaves = ws.GetComponentsInChildren<AbstractWave>();
        DeserializedWaveSpawner.DeserializedWave[] deserializedWaves = new DeserializedWaveSpawner.DeserializedWave[existingWaves.Length];



        Dictionary<string, AbstractWave> wavePrefabPool = new Dictionary<string, AbstractWave>();

        for (int i = 0; i < existingWaves.Length;i++)
        {
            
            AbstractWave ewave = existingWaves[i];

            //Load prefab into pool (to be able to compare values)
            if (!wavePrefabPool.ContainsKey(ewave.name))
            {
                // load prefab
                AbstractWave prefabObject = Resources.Load(prefabsFolder + ewave.name, typeof(AbstractWave)) as AbstractWave;
                Debug.Log("Loaded: " + prefabObject + ", from: " + prefabsFolder + ewave.name);
                // if unsuccesful, error message and jump to next in the foreach loop
                if (prefabObject == null)
                {
                    Debug.LogError("Prefab \"" + ewave.name + "\" does not exists.");
                    continue;
                }

                // otherwise add to dictionary
                wavePrefabPool.Add(ewave.name, prefabObject);
            }
            AbstractWave prefab = wavePrefabPool[ewave.name];



            DeserializedWaveSpawner.DeserializedWave wave;
            if (ewave.GetType() == typeof(NormalWave))
            {
                //If it is a more specialised class of abstractwave, add those parameters aswell.
                DeserializedWaveSpawner.DeserializedNormalWave newWave = new DeserializedWaveSpawner.DeserializedNormalWave();
                newWave.framesInBetween = DeserializedWaveSpawner.toStringNullWhenEqual(((NormalWave)prefab).framesInBetween, ((NormalWave)ewave).framesInBetween);
                wave = newWave;
            }
            else
            {
                wave = new DeserializedWaveSpawner.DeserializedWave();
            }


            wave.prefab = ewave.name;
            wave.startPosition = DeserializedWaveSpawner.getOverwritingSVector3(prefab.startPosition,ewave.startPosition);

            //Only save amountPerType if it is different from the prefab.
            if (!prefab.amountPerType.SequenceEqual(ewave.amountPerType))
            {
                //Debug.Log("Amount per type is NOT equal to that of prefab");
                wave.amountPerType = ewave.amountPerType.Select(x => x.ToString()).ToArray();
            }
            else
            {
                //Debug.Log("Amount per type is equal to that of prefab");
                wave.amountPerType = null;
            }

            wave.difficulty = DeserializedWaveSpawner.toStringNullWhenEqual(prefab.difficulty,ewave.difficulty);
            wave.creepStupidity = DeserializedWaveSpawner.toStringNullWhenEqual(prefab.creepStupidity,ewave.creepStupidity);

            deserializedWaves[i] = wave;
        }

        dws.waves = deserializedWaves;

        XmlIO.SaveXml<DeserializedWaveSpawner>(dws, "./Assets/Resources/" + DeserializedWaveSpawner.xmlExportName + ".xml");
    }
}
