using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.auth.login
{
    public record AuthLoginCommand 
    (
        string Email,
        string Password
    ):IRequest<Result<AuthLoginResponse>>;
}