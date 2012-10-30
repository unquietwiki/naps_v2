// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace NAPS
{
    class CSettings
    {
        private const string PROFILES_FILE = "profiles.xml";

        public static List<CScanSettings> LoadProfiles()
        {
            if (File.Exists(Application.StartupPath + "\\" + PROFILES_FILE))
            {
                Stream strFile = File.OpenRead(Application.StartupPath + "\\" + PROFILES_FILE);
                XmlSerializer serializer = new XmlSerializer(typeof(List<CScanSettings>));
                List<CScanSettings> ret = (List<CScanSettings>)serializer.Deserialize(strFile);
                strFile.Close();
                return ret;
            }
            else
            {
                return new List<CScanSettings>();
            }
        }

        public static void SaveProfiles(List<CScanSettings> profiles)
        {
            Stream strFile = File.Open(Application.StartupPath + "\\" + PROFILES_FILE,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(List<CScanSettings>));
            serializer.Serialize(strFile, profiles);
            strFile.Close();
        }
    }
}
