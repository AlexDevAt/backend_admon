namespace backend_admon.features.tipos_productos.crear_tipo_producto
{
    public record CrearTipoProductoResponse(
        bool status,
        string message,
        CrearTipoProductoDTO tipo_producto
    );
}
