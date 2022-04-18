
using Microsoft.AspNetCore.Http;

namespace DowntimeKuma.S3
{
    public interface IS3Service
    {
        public string Token { get; set; }
        public S3Storage Storage { get; set; }
        public bool HasSession { get; set; }
        public IRequestCookieCollection Cookies { get; set; }
    }
}
