using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DeserializedLevelSaver
{
    public static void saveWaveSpawner(WaveSpawner ws)
    {
        DeserializedWaveSpawner dws = new DeserializedWaveSpawner();
        dws.timeBetweenWaves = ws.timeBetweenWaves.ToString();
        dws.maxTimePerWave = ws.maxTimePerWave.ToString();
        dws.endless = ws.endless.ToString();
        dws.wave = ws.wave.ToString();
        dws.difficulty = ws.difficulty.ToString();
        dws.startPosition = new DeserializedWaveSpawner.SVector3(ws.startPosition);
        dws.targetPosition = new DeserializedWaveSpawner.SVector3(ws.targetPosition);

        AbstractWave[] existingWaves = ws.GetComponentsInChildren<AbstractWave>();
        DeserializedWaveSpawner.DeserializedWave[] deserializedWaves = new DeserializedWaveSpawner.DeserializedWave[existingWaves.Length];

        for (int i = 0; i < existingWaves.Length;i++)
        {
            DeserializedWaveSpawner.DeserializedWave wave = new DeserializedWaveSpawner.DeserializedWave();
            AbstractWave ewave = existingWaves[i];
            wave.startPosition = new DeserializedWaveSpawner.SVector3(ewave.startPosition);
            wave.amountPerType = ewave.amountPerType.Select(x => x.ToString()).ToArray();
            wave.difficulty = ewave.difficulty.ToString();
            wave.creepStupidity = ewave.creepStupidity.ToString();

            deserializedWaves[i] = wave;
        }

        dws.waves = deserializedWaves;

        XmlIO.SaveXml<DeserializedWaveSpawner>(dws, "./Assets/Resources/" + DeserializedWaveSpawner.xmlExportName + ".xml");
    }
}
