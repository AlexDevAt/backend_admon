using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace backend_admon.features.user.registerUser
{
    public static class UserRegisterEndpoint
    {
        public static void MapRegisterUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/users/register", async (UserRegisterCommand command, IMediator mediator) =>
            {
               var response = await mediator.Send(command);
                if (!response.IsSuccess)
                {
                    return Results.Json(data: new
                    {
                       succes=response.IsSuccess,
                       error_message= response.ErrorMessage
                    },statusCode:response.statusCode);
                }
               return Results.Ok(response.Value);
            });
        }
    }
}