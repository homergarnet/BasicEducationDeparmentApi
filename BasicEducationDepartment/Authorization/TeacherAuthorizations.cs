using BasicEducationDepartment.App_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace BasicEducationDepartment.Authorization
{
    public class TeacherAuthorizations : Attribute, IAuthenticationFilter
    {

        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                return;
            }


            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateToken(token, context);

            if (principal == false)
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        protected Task<Boolean> AuthenticateToken(string token, HttpAuthenticationContext context)
        {
            if (ValidateToken(token))
            {
                string apikey = new AuthTokenParser(context.Request).ReadValue("api_key");
                string apisecret = new AuthTokenParser(context.Request).ReadValue("api_secret");
                string usertype = new AuthTokenParser(context.Request).ReadValue("usertype");
                if (apikey == "bed" && apisecret == "bedsecret" && usertype == "teacher")
                    return Task.FromResult<Boolean>(true);

            }

            return Task.FromResult<Boolean>(false);
        }

        private bool ValidateToken(string token)
        {
            var simplePrinciple = JwtManager.GetPrincipal(token);

            if (simplePrinciple == null)
                return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;
            // More validate to check whether username exists in system

            return true;
        }

    }
}