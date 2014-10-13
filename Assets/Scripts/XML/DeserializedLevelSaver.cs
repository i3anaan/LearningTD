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
        saveToFields(ws, instance);

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
        Type createdType = getSubTypeFullCascading(go, mb);
        Debug.Log("Gotten dynamic type: " + createdType);
        ab.Save(aName.Name + ".dll");

        return createdType;
    }

    public static void addFields(System.Object obj, TypeBuilder tb, ModuleBuilder mb)
    {
        foreach (FieldInfo f in obj.GetType().GetFields())
        {
            var valueToStore = f.GetValue(obj);
            
            if (valueToStore != null)
            {
                if (f.FieldType.IsPrimitive || f.FieldType == typeof(string))
                {
                    Debug.Log("Create field: " + f);
                    FieldBuilder fb = tb.DefineField(
                    f.Name,
                    f.FieldType,
                    FieldAttributes.Public);
                }
                else if (!f.FieldType.IsArray) 
                {
                    //Non primitive Field > New dynamic class needs to be made.

                    Type createdType = getSubTypeFullCascading(valueToStore, mb);
                    Debug.Log("Gotten dynamic type: " + createdType);
                    FieldBuilder fb = tb.DefineField(
                    f.Name,
                    createdType,
                    FieldAttributes.Public);
                }
                else//Case = Array           - TODO arrays in arrays
                {
                    Type createdType = getSubType(f.FieldType.GetElementType(), mb);
                    //This createdType has NO fields.
                    //Later on whenever this type gets encountered new classes should be made that inherit from this empty base type.
                    FieldBuilder fb = tb.DefineField(
                    f.Name,
                    createdType.MakeArrayType(),
                    FieldAttributes.Public);
                }
            }
        }
    }

    public static void saveToFields(System.Object originalObj, System.Object serializableObj)
    {
        foreach (FieldInfo f in originalObj.GetType().GetFields())
        {
            var valueToStore = f.GetValue(originalObj);
            Debug.Log("Value to store in " + f + " is: " + valueToStore);
            if (valueToStore != null)
            {
                if ((f.FieldType.IsPrimitive || f.FieldType == typeof(string)))
                {
                    serializableObj.GetType().GetField(f.Name).SetValue(serializableObj, valueToStore);
                }
                else if (!f.FieldType.IsArray)
                {
                    Debug.Log("Field to store = " + f.Name);
                    Debug.Log("Creating new instance of: " + serializableObj.GetType().GetField(f.Name).FieldType);
                    var newNonPrimitiveSerializableToStore = Activator.CreateInstance(serializableObj.GetType().GetField(f.Name).FieldType);
                    saveToFields(f.GetValue(originalObj), newNonPrimitiveSerializableToStore);
                    serializableObj.GetType().GetField(f.Name).SetValue(serializableObj, newNonPrimitiveSerializableToStore);
                }
                else
                {
                    Debug.LogError("TODO: arrays, see code");
                    //When it finds an array, it needs to go through all the actual instances in that array, make new classes for them, and let them extend the (empty) base type the serializableObj has in its field.
                    //After that, need to figure out a way to know which class it is when loading.
                    //And need to compare to prefab, to massively reduce the size of the xml file (and improve readability)
                }
            }
        }
    }

    public static Type getSubTypeFullCascading(System.Object obj, ModuleBuilder mb)
    {
        Type type = mb.GetType("Deserialised_" + obj.GetType().Name); 
        if (type == null)
        {
            Debug.Log("CreateSubTypeFullCascading for: " + obj.GetType().Name);
            TypeBuilder tb = mb.DefineType(
                "Deserialised_" + obj.GetType().Name,
                 TypeAttributes.Public);
            addFields(obj, tb,mb);
            return tb.CreateType();
        }
        else
        {
            return type;
        }
    }

    //This will, in contrast to FullCascading, not add fields to the dynamicly made type.
    public static Type getSubType(Type type, ModuleBuilder mb)
    {
        Type foundType = mb.GetType("Deserialised_" + type.Name);
        if (foundType == null)
        {
            Debug.Log("CreateSubType for: " + type.Name);
            TypeBuilder tb = mb.DefineType(
                "Deserialised_" + type.Name,
                 TypeAttributes.Public);
            return tb.CreateType();
        }
        else
        {
            return foundType;
        }
    }


}
