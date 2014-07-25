using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Amigo.Utils
{
    public class AcceptAllCertificatePolicy : ICertificatePolicy
    {
        public AcceptAllCertificatePolicy()
        {
            // Nothing to do.
        }

        public bool CheckValidationResult(ServicePoint srvPoint,
        X509Certificate certificate, WebRequest request,
        int certificateProblem)
        {
            // Just accept.
            return true;
        }
    }
}
