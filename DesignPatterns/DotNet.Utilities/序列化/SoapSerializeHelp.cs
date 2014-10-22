/*
 * Author: XYZ.SEAN.M.THX 
 * Email: seanmpthx@gmail.com
 * Copyright: (c) 2010-2020
 * Version:0.1
 * 
 * <Update>
 *   2012-06-05 XYZ.SEAN.M.THX 创建
 * </Update>
 * <Remark>
 * 
 * </Remark>
*/
using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace DotNet.Utilities
{
    public class SoapSerializeHelp
    {
        /// <summary>
        /// SoapFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToSoap<T>(T item)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        /// <summary>
        /// SoapFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromSoap<T>(string str)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
