namespace backend_admon.features.user.registerUser
{
    public record UserRegisterResponse
    (
        Guid Id_public,
        string Nombre_usuario,
        string Email,
        DateTime Create_at
    );

    
}