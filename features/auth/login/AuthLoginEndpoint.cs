using MediatR;

namespace backend_admon.features.auth.login
{
    public static class AuthLoginEndpoint
    {
        
         public static void MapAuthLoginEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/login", async (AuthLoginCommand command, IMediator mediator) =>
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