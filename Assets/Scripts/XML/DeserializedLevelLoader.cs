using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using ExtensionMethods;

public class DeserializedLevelLoader
{
    public static void updateWaveSpawner()
    {
        WaveSpawner existingWS = GameObject.FindObjectOfType<WaveSpawner>();
        DeserializedWaveSpawner dws = XmlIO.LoadXml<DeserializedWaveSpawner>("WaveSpawner");

        existingWS.timeBetweenWaves = int.Parse(dws.timeBetweenWaves);
        existingWS.maxTimePerWave = int.Parse(dws.maxTimePerWave);
        existingWS.endless = bool.Parse(dws.endless);
        existingWS.startWave = int.Parse(dws.startWave);
        existingWS.difficulty = int.Parse(dws.difficulty);
        existingWS.startPosition = dws.startPosition.toVector3();
        if (dws.targetPosition != null)
        {
            existingWS.targetPosition = dws.targetPosition.toVector3();
        }
        existingWS.waves = new AbstractWave[dws.waves.Length];
        for(int i=0;i<dws.waves.Length;i++)
        {
            DeserializedWaveSpawner.DeserializedWave wave = dws.waves[i];
            AbstractWave lwave = Resources.Load(DeserializedLevelSaver.prefabsFolder + wave.prefab,typeof(AbstractWave)) as AbstractWave;
            Debug.Log(lwave);
            Debug.Log(DeserializedLevelSaver.prefabsFolder + wave.prefab);
            if (lwave.GetType() == typeof(NormalWave))
            {
                if (((DeserializedWaveSpawner.DeserializedNormalWave)wave).framesInBetween != null)
                {
                    ((NormalWave)lwave).framesInBetween = int.Parse(((DeserializedWaveSpawner.DeserializedNormalWave)wave).framesInBetween);
                }
            }

            var ints = new Int32[] { 3, 4, 5 };
            //Debug.Log("Parse test: "+ ints.Parse(new String[2] { "1", "2" }));
            //Debug.Log("ExtensionMethods: " + typeof(ParseExtensionMethods).GetMethods().Length);
            foreach (MethodInfo mi in typeof(ParseExtensionMethods).GetMethods())
            {
                //Debug.Log("ExtensionMethod found: " + mi.Name);
            }

            //For each field present, check if it is null, if not parse and save (update) it in the newly made lwave
            FieldInfo[] fields = wave.GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                if (f.GetValue(wave) != null)
                {
                    Debug.Log("Processing field: " + f.Name);
                    FieldInfo fieldToUpdate = lwave.GetType().GetField(f.Name);
                    if (fieldToUpdate != null)
                    {
                        //Debug.Log("Value to parse: " + f.GetValue(wave));
                        //Debug.Log("FieldToUpdate: " + fieldToUpdate);
                        var fieldType = fieldToUpdate.FieldType;
                        //Debug.Log("FieldType: " + fieldType);
                        Debug.Log("Found " + fieldType.GetMethods().Length + " methods");
                        foreach (MethodInfo mi in fieldType.GetMethods())
                        {
                            Debug.Log("Method found: " + mi.Name);
                        }
                        var parseMethod = fieldType.GetMethod("Parse", new Type[] { "".GetType()});
                        if (parseMethod == null)
                        {
                            parseMethod = typeof(ParseExtensionMethods).GetMethod("Parse", new Type[] { fieldType, new string[0].GetType() });
                            //Debug.Log("New parseMethod: " + parseMethod);
                            //Debug.Log(f.GetValue(wave));
                            var loadedValue = parseMethod.Invoke(null, new object[2] { null, f.GetValue(wave) });
                            //Debug.Log("LoadedValue: "+loadedValue);
                            fieldToUpdate.SetValue(lwave, loadedValue);
                        }
                        else
                        {
                            var loadedValue = parseMethod.Invoke(null, new object[1] { f.GetValue(wave) });
                            fieldToUpdate.SetValue(lwave, loadedValue);
                        }
                        //Debug.Log("ParseMethod: "+parseMethod);
                        
                        
                        
                    }
                }
            }
            GameObject.DestroyImmediate(existingWS.waves[i]);
            existingWS.waves[i] = lwave;
            lwave.transform.parent = existingWS.transform;
        }
    }
}


