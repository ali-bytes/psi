using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace NewIspNL
{
    [Serializable]
    public class Js
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string[] Path { get; set; }
    }

    public class Css
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string[] Path { get; set; }
    }

    [Serializable]
    public class Bundling
    {
        [XmlElement]
        public Js[] Js { get; set; }

        [XmlElement]
        public Css[] Css { get; set; }
    }
}