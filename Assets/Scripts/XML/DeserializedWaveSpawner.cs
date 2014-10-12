using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System;
using System.Globalization;

[XmlRoot("WaveSpawner")]
public class DeserializedWaveSpawner
{
    public static string xmlExportName = "WaveSpawner";

    [XmlElement("Time_between_waves")]
    public int timeBetweenWaves;
    [XmlElement("Max_time_per_wave")]
    public string maxTimePerWave;
    [XmlElement("Endless")]
    public string endless;
    [XmlElement("Start_wave")]
    public string startWave;
    [XmlElement("Difficulty")]
    public string difficulty;
    [XmlElement("Start_position")]
    public SVector3 startPosition;
    [XmlElement("Target_position")]
    public SVector3 targetPosition;

    //[XmlElement("Target_position")]
    //public string[] waves;



    public DeserializedWave[] waves;
    [XmlInclude(typeof(DeserializedNormalWave))]
    public class DeserializedWave
    {
        public string prefab;

        public SVector3 startPosition;
        public DeserializedCreep[] creepTypes;
        public string[] amountPerType;
        public string difficulty;
        public string creepStupidity;
    }

    public class DeserializedNormalWave : DeserializedWave
    {
        public string framesInBetween;
    }

    public class DeserializedCreep
    {

    }




    public class SVector3
    {
        public SVector3()
        {
        }
        [XmlAttribute("x")]
        public string x;
        [XmlAttribute("y")]
        public string y;
        [XmlAttribute("z")]
        public string z;

        public Vector3 toVector3()
        {
            return new Vector3(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture), float.Parse(z, CultureInfo.InvariantCulture));
        }
    }
    public static SVector3 getOverwritingSVector3(Vector3 overwritingVec)
    {
        return getOverwritingSVector3(new Vector3(),overwritingVec);
    }
    
    public static SVector3 getOverwritingSVector3(Vector3 baseVec, Vector3 overwritingVec)
    {
        if (baseVec == overwritingVec)
        {
            return null;
        }
        else
        {
            SVector3 output = new SVector3();
            output.x = overwritingVec.x.ToString();
            output.y = overwritingVec.y.ToString();
            output.z = overwritingVec.z.ToString();
            //output.x = toStringNullWhenEqual(baseVec.x, overwritingVec.x);
            //output.y = toStringNullWhenEqual(baseVec.y, overwritingVec.y);
            //output.z = toStringNullWhenEqual(baseVec.z, overwritingVec.z);
            return output;
        }
    }

    public static string toStringNullWhenZero(string s)
    {
        return toStringNullWhenEqual("0", s);
    }

    public static string toStringNullWhenEqual<T>(T baseS, T overwritingS)
    {
        return baseS.Equals(overwritingS) ? null : overwritingS.ToString();
    }
}
