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
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DotNet.Utilities
{
    public class XMLSerializeHelp
    {
        /// <summary>
        /// 文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            using (fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            using(fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToXml<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromXml<T>(string filename)
        {
            FileStream fs = null;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {              
                return (T)serializer.Deserialize(fs);
            }
        }
    }
}
