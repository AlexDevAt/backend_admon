using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.categorias.crear_categoria
{
    public record CrearCategoriaCommand(string name) : IRequest<Result<CrearCategoriaResponse>>;
}
