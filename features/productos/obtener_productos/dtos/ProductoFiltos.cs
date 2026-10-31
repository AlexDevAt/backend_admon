namespace backend_admon.features.productos.obtener_productos.dtos
{
    public record ProductoFiltros
    {
        public string? categoria { get; init; }
        public int? tipo { get; init; }
        public int? compania { get; init; }

        public decimal? precioMin { get; init; }
        public decimal? precioMax { get; init; }

        public bool? disponible { get; init; }

        public string? expansiones { get; init; }
        public string? rarezas { get; init; }
        public string? condiciones { get; init; }

        public string? orden { get; init; }

        public int? limite { get; init; }
        public string? busqueda { get; init; }
    }
}
