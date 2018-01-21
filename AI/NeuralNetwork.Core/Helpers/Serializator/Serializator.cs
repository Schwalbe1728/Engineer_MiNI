using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NeuralNetwork.Core.Learning;

namespace NeuralNetwork.Core.Helpers.Serializator
{
    public static class Serializator
    {
        public static bool Serialize(this LearningProcess process, string path)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(process.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, process);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(path);
                    stream.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error while serializing object: " + e.Message);
            }
            return false;
        }

        public static LearningProcess Deserialize(string path)
        {
            if (string.IsNullOrEmpty(path)) { return null; }

            try
            {
                LearningProcess result = null;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(LearningProcess));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        result = (LearningProcess)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
                if (result != null)
                {
                    result.LearningAlgorithm.Config.Reinitialize();
                }

                return result;
            }
            catch (Exception e)
            {
                 Debug.WriteLine("There was an error while deserializing object: " + e.Message);
            }

            return null;
        }
    }
}