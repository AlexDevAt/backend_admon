namespace backend_admon.features.user.registerUser
{
    public record UserRegisterResponse
    (
        Guid id_public,
        string Nombre,
        String Email

    );
}