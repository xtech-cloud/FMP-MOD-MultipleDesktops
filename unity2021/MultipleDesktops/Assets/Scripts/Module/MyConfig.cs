
using System.Xml.Serialization;

namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class Splash
        {
            [XmlAttribute("image")]
            public string image { get; set; } = "";
        }
        public class BackButton : UiElement
        {
            [XmlArray("SubjectS"), XmlArrayItem("Subject")]
            public Subject[] subjectS = new Subject[0];
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("color")]
            public string color { get; set; } = "#00000000";
            [XmlElement("BackButton")]
            public BackButton backButton { get; set; } = null;
            [XmlElement("Splash")]
            public Splash splash { get; set; } = null;
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

