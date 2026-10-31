using backend_admon.features.productos.obtener_productos.dtos;

namespace backend_admon.features.productos.obtener_productos
{
    public record ObtenerProductosResponse<T>(bool status, int total_productos, List<T>? productos);
}
