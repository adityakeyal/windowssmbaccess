using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsSMBAccess
{
    class XmlOptions
    {


        public static void Save(Options options) {


            XmlSerializer serializer = new XmlSerializer(typeof(Options));
            StringWriter sr = new StringWriter();
            serializer.Serialize(sr, options);
            

        }

        public static Options Read()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Options));

            using (FileStream stream = new FileStream("sample.xml",FileMode.Open)) {
                return (Options)serializer.Deserialize(stream);
            }
        }
    }


    [XmlRoot("Options")]
    public class Options {

        [XmlElement("Option")]
        public List<Option> Ops { get; set; }


        public Options() => Ops = new List<Option>();

    }


public    class Option {

        [XmlElement("Op")]
        public string Op { get; set; }

        [XmlElement("Directory")]
        public string Directory { get; set; }

        [XmlElement("Day")]
        public int Day { get; set; }

        [XmlElement("User")]
        public string User { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }


    }
}
