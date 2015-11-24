using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MinesWeeper.Model
{
    static class ExtensionMethods
    {
        public static string SerializeToXml<T>(this T serizalizingObject)
        {
            string xml;

            if (!typeof(T).IsSerializable)
                throw new ArgumentException("Passing object is not serializable");

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                    xs.Serialize(xw, serizalizingObject);
                    xml = sw.ToString();              
            }

            return xml.ToString();

        }       
               

    }
}
