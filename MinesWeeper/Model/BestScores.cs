using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MinesWeeper.Model
{
    //class
    class BestScores
    {
        public string FileName { get; set; } = "MinesWeeperBS.xml";
        public List<Score> ScoresCollection { get; private set; } 

        public BestScores()
        {
            ScoresCollection = new List<Score>();
            
        }

        public List<Score> LoadScores()
        {
           try{               

                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    var ds = new XmlSerializer(typeof(Score));
                    var xr = XmlReader.Create(fs);
                }                        

            }catch(Exception e)
            {
                throw e;
            }

            return new List<Score>();

        }
      
        public void UpdateScores()
        {

            var xmlheader = new XmlDocument().CreateXmlDeclaration("1.0", "utf-8", "yes");
            var xml = "<?xml version=\"1.0\" ?>" + 
                       ScoresCollection.Select(x => x.SerializeToXml())
                                       .Aggregate((i,j) => i + "\n" + j);
    
            Debug.Write(xml);
            try
            {
                //Append two xmls
                var z = XDocument.Load(FileName);
                File.AppendAllText(FileName, xml.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }



        }

    }

    //Storage class

    [Serializable]
    public class Score
    {
        public DateTime TimeSave { get; set; }
        public TimeSpan GameTime { get; set; }

        public int GameSize { get; set; }
        public int GameMinesPercentage { get; set; }
    }
}
