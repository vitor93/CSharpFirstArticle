using Microsoft.AspNetCore.Authorization;
using SharedModels.User.Enum;

namespace StocksApi.Utils.AuthorizationRoles;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params UserTypeEnum[] userTypes): base()
    {
        Roles = string.Join(",", userTypes);
    }
}
