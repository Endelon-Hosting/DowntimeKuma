
using Logging.Net;

using Microsoft.AspNetCore.Http;

using System;
using System.Net.Http;

namespace DowntimeKuma.S3
{
    public class S3Service : IS3Service
    {
        public string Token { get; set; } = "unset";
        public S3Storage Storage { get; set; }
        public bool HasSession { get; set; }
        public HttpContext HttpContext { get; set; }
        public IRequestCookieCollection Cookies { get; set; }

        public S3Service(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;

            if (HttpContext != null)
            {
                var request = HttpContext.Request;
                string path = request.Path;

                Cookies = HttpContext.Request.Cookies;
                string token = HttpContext.Request.Cookies["s3token"];

                if (string.IsNullOrEmpty(token))
                {
                    Token = "null";
                    Storage = null;
                    HasSession = false;
                }
                else
                {
                    if (S3Storage.IsTokenValid(token))
                    {
                        Token = token;
                        HasSession = true;
                        Storage = S3Storage.Get(token);
                    }
                    else
                    {
                        Token = token;
                        Storage = null;
                        HasSession = false;
                    }
                }
            }
        }
    }
}
