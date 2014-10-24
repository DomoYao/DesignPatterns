
namespace DotNet.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 反射助手
    /// </summary>
    public static class ReflectionHelper
    {
        #region Private Constants
        private const int InitialPrime = 23;
        private const int FactorPrime = 29;
        #endregion

        #region Extension Methods
        /// <summary>
        /// Gets the signature string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The signature string.</returns>
        public static string GetSignature(this Type type)
        {
            StringBuilder sb = new StringBuilder();

            if (type.IsGenericType)
            {
                sb.Append(type.GetGenericTypeDefinition().FullName);
                sb.Append("[");
                int i = 0;
                var genericArgs = type.GetGenericArguments();
                foreach (var genericArg in genericArgs)
                {
                    sb.Append(genericArg.GetSignature());
                    if (i != genericArgs.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            else
            {
                if (!string.IsNullOrEmpty(type.FullName))
                    sb.Append(type.FullName);
                else if (!string.IsNullOrEmpty(type.Name))
                    sb.Append(type.Name);
                else
                    sb.Append(type.ToString());

            }
            return sb.ToString();
        }
        /// <summary>
        /// Gets the signature string.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The signature string.</returns>
        public static string GetSignature(this MethodInfo method)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ReturnType.GetSignature());
            sb.Append(" ");
            sb.Append(method.Name);
            if (method.IsGenericMethod)
            {
                sb.Append("[");
                var genericTypes = method.GetGenericArguments();
                int i = 0;
                foreach (var genericType in genericTypes)
                {
                    sb.Append(genericType.GetSignature());
                    if (i != genericTypes.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            sb.Append("(");
            var parameters = method.GetParameters();
            if (parameters != null && parameters.Length > 0)
            {
                int i = 0;
                foreach (var parameter in parameters)
                {
                    sb.Append(parameter.ParameterType.GetSignature());
                    if (i != parameters.Length - 1)
                        sb.Append(", ");
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
        /// <summary>
        /// Deserializes an object from the given byte stream.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="type">The type of the object to be deserialized.</param>
        /// <param name="stream">The byte stream that contains the data of the object.</param>
        /// <returns>The deserialized object.</returns>
        public static object Deserialize(this IObjectSerializer serializer, Type type, byte[] stream)
        {
            var deserializeMethodInfo = serializer.GetType().GetMethod("Deserialize");
            return deserializeMethodInfo.MakeGenericMethod(type).Invoke(serializer, new object[] { stream });
        }
        /// <summary>
        /// Get GetCustomAttribute
        /// </summary>
        /// <typeparam name="T">CustomAttribute Type</typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? attributes[0] as T : default(T);
        }

        
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the hash code for an object based on the given array of hash
        /// codes from each property of the object.
        /// </summary>
        /// <param name="hashCodesForProperties">The array of the hash codes
        /// that are from each property of the object.</param>
        /// <returns>The hash code.</returns>
        public static int GetHashCode(params int[] hashCodesForProperties)
        {
            unchecked
            {
                int hash = InitialPrime;
                foreach (var code in hashCodesForProperties)
                    hash = hash * FactorPrime + code;
                return hash;
            }
        }
        /// <summary>
        /// Generates a unique identifier represented by a <see cref="System.String"/> value
        /// with the specified length.
        /// </summary>
        /// <param name="length">The length of the identifier to be generated.</param>
        /// <returns>The unique identifier represented by a <see cref="System.String"/> value.</returns>
        public static string GetUniqueIdentifier(int length)
        {
            int maxSize = length;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            // Unique identifiers cannot begin with 0-9
            if (result[0] >= '0' && result[0] <= '9')
            {
                return GetUniqueIdentifier(length);
            }
            return result.ToString();
        }
        #endregion

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="classFullName">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string classFullName)
        {
            try
            {
                object ect = Assembly.Load(assemblyName).CreateInstance(classFullName);
                return (T)ect;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static MemberInfo GetProperty(LambdaExpression lambda)
        {
            Expression expression = lambda;
            for (; ; )
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Lambda:
                        expression = ((LambdaExpression)expression).Body;
                        break;
                    case ExpressionType.Convert:
                        expression = ((UnaryExpression)expression).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expression;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }
    }
}
