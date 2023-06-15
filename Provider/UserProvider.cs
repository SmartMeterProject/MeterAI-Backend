using System.Security.Claims;

namespace Counter.Provider
{
    public class UserProvider:IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserId()
        {
            var userIdClaim = _context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
            else
            {
                var deneme = _context.HttpContext.User.Claims;
                // Print out all claims for debugging
                foreach (var claim in _context.HttpContext.User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }

                // Handle the case when the user ID claim is not found
                return null;
            }
        }
    }
}
