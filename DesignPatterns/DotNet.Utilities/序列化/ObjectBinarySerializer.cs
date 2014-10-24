using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNet.Utilities
{
    /// <summary>
    /// Represents the binary serializer.
    /// </summary>
    public class ObjectBinarySerializer : IObjectSerializer
    {
        #region Private Fields
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();
        #endregion

        #region IObjectSerializer Members
        /// <summary>
        /// Serializes an object into a byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte stream which contains the serialized data.</returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                binaryFormatter.Serialize(ms, obj);
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
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)binaryFormatter.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
