using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using System.Globalization;

[XmlRoot("WaveSpawner")]
public class DeserializedWaveSpawner
{
    public static string xmlExportName = "WaveSpawner";

    [XmlElement("Time_between_waves")]
    public string timeBetweenWaves;
    [XmlElement("Max_time_per_wave")]
    public string maxTimePerWave;
    [XmlElement("Endless")]
    public string endless;
    [XmlElement("Start_wave")]
    public string wave;
    [XmlElement("Difficulty")]
    public string difficulty;
    [XmlElement("Start_position")]
    public SVector3 startPosition;
    [XmlElement("Target_position")]
    public SVector3 targetPosition;

    public DeserializedWave[] waves;
    public class DeserializedWave
    {
        public SVector3 startPosition;
        public DeserializedCreep[] creepTypes;
        public string[] amountPerType;
        public string difficulty;
        public string creepStupidity;
    }

    public class DeserializedCreep
    {

    }




    public class SVector3
    {
        public SVector3()
        {
            this.x = "0";
            this.y = "0";
            this.z = "0";
        }

        public SVector3(Vector3 vec3)
        {
            this.x = vec3.x.ToString();
            this.y = vec3.y.ToString();
            this.z = vec3.z.ToString();
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
}
