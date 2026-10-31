using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.tipos_productos.crear_tipo_producto
{
    public record CrearTipoProductoCommand(string name)
        : IRequest<Result<CrearTipoProductoResponse>>;
}
