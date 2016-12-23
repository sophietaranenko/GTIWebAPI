using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Novell;
using GTIWebAPI.Models.Context;
using Microsoft.Owin;
using System.Net;

namespace GTIWebAPI.Providers
{

    public class Constants
    {
        public const string OwinChallengeFlag = "X-Challenge";
    }
    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
            if (context.Response.StatusCode == 400 && context.Response.Headers.ContainsKey(Constants.OwinChallengeFlag))
            {
                var headerValues = context.Response.Headers.GetValues(Constants.OwinChallengeFlag);
                context.Response.StatusCode = Convert.ToInt16(headerValues.FirstOrDefault());
                context.Response.Headers.Remove(Constants.OwinChallengeFlag);
            }

        }
    }

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        /// <summary>
        /// Ctor of oAuth provider
        /// </summary>
        /// <param name="publicClientId"></param>
        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
        }

        /// <summary>
        /// First - NovellProvider, then OAuth Provider
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            NovellManager novellManager = new NovellManager("192.168.0.1", userManager);

            ApplicationUser user = novellManager.Find(context.UserName, context.Password);

            if (user == null)
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                //была проблема с возвращением 400
                //а надо было возвращать 401 
                //пришлось решить
                context.Response.Headers.Add(Constants.OwinChallengeFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
                return;
            }
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);
            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        /// <summary>
        /// Where dp we get the token 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// If everything is OK, we mark context as validated 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Where do we need to return 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Creating application properties (adding username to context)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userName ) 
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}