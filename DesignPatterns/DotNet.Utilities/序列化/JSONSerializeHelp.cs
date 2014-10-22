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
using System.Runtime.Serialization.Json;
using System.IO;

namespace DotNet.Utilities
{
    public class JSONSerializeHelp
    {
        /// <summary>
        /// JsonSerializer序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToJson<T>(T item)
        {
            if (item==null)
            {
                return "";
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// JsonSerializer反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromJson<T>(string str) where T : class
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                return serializer.ReadObject(ms) as T;
            }
        }
     
    }
}
