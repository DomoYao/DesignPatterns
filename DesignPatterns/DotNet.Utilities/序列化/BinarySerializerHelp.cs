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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DotNet.Utilities
{
    public class BinarySerializerHelp
    {
        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToBinary<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromBinary<T>(string str)
        {
            int intLen = str.Length / 2;
            byte[] bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
