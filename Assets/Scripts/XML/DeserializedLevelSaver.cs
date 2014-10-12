using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


public class DeserializedLevelSaver
{
    public static string prefabsFolder = "Prefabs/Waves/";


    public static void saveWaveSpawner(WaveSpawner ws)
    {
        
        /*
        DeserializedWaveSpawner dws = new DeserializedWaveSpawner();
        dws.timeBetweenWaves = ws.timeBetweenWaves;
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
        */
        
        Type deserializedType = buildDeserializedTypeFor(ws);
        System.Object instance = Activator.CreateInstance(deserializedType);

        //showTypeDebug(deserializedType);

        XmlIO.SaveXml<System.Object>(instance, "./Assets/Resources/" + DeserializedWaveSpawner.xmlExportName + ".xml");
   
 }

    public static void showTypeDebug(Type type)
    {
        foreach (FieldInfo f in type.GetFields())
        {
            Debug.Log("Field:  " + f);
            if (f.FieldType.IsArray && !f.FieldType.GetElementType().IsPrimitive)
            {
                Debug.Log("Non primitive field type found");
                showTypeDebug(f.FieldType.GetElementType());
            }
        }
    }






    public static Type buildDeserializedTypeFor(System.Object go)
    {
        AssemblyName aName = new AssemblyName("DeserializedXMLAssembly");
        AssemblyBuilder ab =
            AppDomain.CurrentDomain.DefineDynamicAssembly(
                aName,
                AssemblyBuilderAccess.RunAndSave);

        // For a single-module assembly, the module name is usually 
        // the assembly name plus an extension.
        ModuleBuilder mb =
            ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");
        Type createdType = createSubType(go.GetType(), mb);
        ab.Save(aName.Name + ".dll");

        return createdType;
    }

    public static void addFieldsOrSubclasses(Type type, ModuleBuilder mb, TypeBuilder tb)
    {
        foreach (FieldInfo f in type.GetFields())
        {
            //Debug.Log("Field found: " + f);
            if (f.FieldType.IsPrimitive || f.FieldType == typeof(string))
            {
                FieldBuilder fb = tb.DefineField(
                f.Name,
                f.FieldType,
                FieldAttributes.Public);
            }
            else if(f.FieldType.IsArray)
            {
                
                //TODO arrays in arrays
                if (f.FieldType.GetElementType().IsPrimitive || f.FieldType.GetElementType()==typeof(string))
                {
                    FieldBuilder fb = tb.DefineField(
                    f.Name,
                    f.FieldType,
                    FieldAttributes.Public);
                }
                else
                {
                    Type createdType = createSubType(f.FieldType.GetElementType(), mb);
                    FieldBuilder fb = tb.DefineField(
                    f.Name,
                    createdType.MakeArrayType(),
                    FieldAttributes.Public);
                    //DEBUG;
                    fb.SetConstant(Activator.CreateInstance(createdType));
                }
            }
            else
            {
                Type createdType = createSubType(f.FieldType, mb);
                FieldBuilder fb = tb.DefineField(
                f.Name,
                createdType,
                FieldAttributes.Public);
                //DEBUG;
                fb.SetConstant(Activator.CreateInstance(createdType));
            }
        }
    }

    public static Type createSubType(Type type, ModuleBuilder mb)
    {
        Debug.Log("Find type for: " + "Deserialised_" + type.Name + " Found: " + Type.GetType("Deserialised_" + type.Name));
        //TODO bijhouden welke al wel en welke nog niet gemaakt zijn.
        if (Type.GetType("Deserialised_" + type.Name) == null)
        {
            Debug.Log("CreateSubType for: " + type);
            TypeBuilder tb = mb.DefineType(
                "Deserialised_" + type.Name,
                 TypeAttributes.Public);
            addFieldsOrSubclasses(type, mb, tb);
            return tb.CreateType();
        }
        else
        {
            return Type.GetType("Deserialised_" + type.Name);
        }
    }
}
