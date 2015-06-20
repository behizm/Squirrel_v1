using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace Squirrel.Utility.Xml
{
    public static class XmlReadService<T> where T : class
    {
        private static List<T> _entity;

        public static List<T> Entity
        {
            get
            {
                if (_entity != null) return _entity;

                try
                {
                    var deserializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(typeof(T).Name + "s"));
                    var xmlFilePath = Path.Combine((string)AppDomain.CurrentDomain.GetData("DataDirectory"), typeof(T).Name + "s.xml");
                    //var root = HttpContext.Current.Server.MapPath("~/App_Data/" + typeof(T).Name + "s.xml");
                    var textReader = new StreamReader(xmlFilePath);
                    _entity = (List<T>)deserializer.Deserialize(textReader);
                    textReader.Close();
                    return _entity;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
