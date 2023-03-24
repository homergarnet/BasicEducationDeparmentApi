using System.Net.Http;
using System.Security.Claims;

namespace BasicEducationDepartment.App_Utility
{
    public class AuthTokenParser
    {
        private ClaimsIdentity _claims = null;
        public AuthTokenParser(HttpRequestMessage request)
        {
            var authorization = request.Headers.Authorization;
            var token = authorization.Parameter;
            var simplePrinciple = JwtManager.GetPrincipal(token);
            _claims = simplePrinciple.Identity as ClaimsIdentity;
        }

        public string ReadValue(string key)
        {
            var claimvalue = _claims.FindFirst(key);
            return claimvalue?.Value;
        }
    }
}