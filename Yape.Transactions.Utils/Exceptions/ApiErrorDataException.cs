using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Transactions.Utils.Exceptions
{
    [Serializable]
    public class ApiErrorDataException : Exception
    {
        public string ErrorCode { get; }

        public ApiErrorDataException(string errorCode, string message)
            : base(message) => ErrorCode = errorCode;

#if NET8_0_OR_GREATER
        [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.",
            DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
        protected ApiErrorDataException(SerializationInfo info, StreamingContext context)
            : base(info, context) => ErrorCode = info.GetString(nameof(ErrorCode));

#if NET8_0_OR_GREATER
        [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.",
            DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode);
        }
    }
}
