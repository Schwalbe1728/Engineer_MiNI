using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcquiringModule
{
    public class SensorData
    {
        public static float KEY_NOT_FOUND { get { return float.MinValue; } }

        private float[] sensors;

        private Dictionary<string, float> additionalData;

        public SensorData(float[] sensorValues)
        {
            sensors = sensorValues;
            additionalData = new Dictionary<string, float>();
        }

        public void InsertData(string key, float value)
        {
            additionalData.Add(key, value);
        }

        public float[] Sensors
        {
            get
            {
                return sensors;
            }
        }

        public float Data(string key)
        {
            return
                (additionalData.ContainsKey(key)) ?
                    additionalData[key] :
                    KEY_NOT_FOUND;
        }
    }
}
