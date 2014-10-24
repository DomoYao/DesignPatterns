using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace DotNet.Utilities
{
    /// <summary>
    /// Represents the Json serializer.
    /// </summary>
    public class ObjectJsonSerializer : IObjectSerializer
    {
        #region IObjectSerializer Members
        /// <summary>
        /// Serializes an object into a byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte stream which contains the serialized data.</returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            Type graphType = obj.GetType();
            DataContractJsonSerializer js = new DataContractJsonSerializer(graphType);
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, obj);
                ret = ms.ToArray();
                ms.Close();
            }
            return ret;
        }
        /// <summary>
        /// Deserializes an object from the given byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The byte stream which contains the serialized data of the object.</param>
        /// <returns>The deserialized object.</returns>
        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(TObject));
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)js.ReadObject(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
