// -------------------------------------------------------------
// By: Erik Portilla
// Date: 2025-08-10
// -------------------------------------------------------------


// -------------------------------------------------------------
// By: Erik Portilla
// Date: 2025-01-10
// Modelos para XML de Factura SRI versión 1.1.0
// -------------------------------------------------------------

using System.Xml.Serialization;

namespace Yamgooo.SRI.Xml.Models;

[XmlRoot("factura", Namespace = "", IsNullable = false)]
public class SriInvoice
{
    [XmlAttribute("id")]
    public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")]
    public string Version { get; set; } = "1.1.0";

    [XmlElement("infoTributaria")]
    public InfoTributaria InfoTributaria { get; set; } = new();

    [XmlElement("infoFactura")]
    public InfoFactura InfoFactura { get; set; } = new();

    [XmlElement("detalles")]
    public Detalles Detalles { get; set; } = new();

    [XmlElement("retenciones")]
    public Retenciones? Retenciones { get; set; }

    [XmlElement("infoAdicional")]
    public InfoAdicional? InfoAdicional { get; set; }
}

public class InfoTributaria
{
    [XmlElement("ambiente")]
    public string Ambiente { get; set; } = string.Empty;

    [XmlElement("tipoEmision")]
    public string TipoEmision { get; set; } = string.Empty;

    [XmlElement("razonSocial")]
    public string RazonSocial { get; set; } = string.Empty;

    [XmlElement("nombreComercial")]
    public string? NombreComercial { get; set; }

    [XmlElement("ruc")]
    public string Ruc { get; set; } = string.Empty;

    [XmlElement("claveAcceso")]
    public string ClaveAcceso { get; set; } = string.Empty;

    [XmlElement("codDoc")]
    public string CodDoc { get; set; } = "01";

    [XmlElement("estab")]
    public string Estab { get; set; } = string.Empty;

    [XmlElement("ptoEmi")]
    public string PtoEmi { get; set; } = string.Empty;

    [XmlElement("secuencial")]
    public string Secuencial { get; set; } = string.Empty;

    [XmlElement("dirMatriz")]
    public string DirMatriz { get; set; } = string.Empty;
}

public class InfoFactura
{
    [XmlElement("fechaEmision")]
    public string FechaEmision { get; set; } = string.Empty;

    [XmlElement("dirEstablecimiento")]
    public string? DirEstablecimiento { get; set; }

    [XmlElement("contribuyenteEspecial")]
    public string? ContribuyenteEspecial { get; set; }

    [XmlElement("obligadoContabilidad")]
    public string? ObligadoContabilidad { get; set; }

    [XmlElement("tipoIdentificacionComprador")]
    public string TipoIdentificacionComprador { get; set; } = string.Empty;

    [XmlElement("guiaRemision")]
    public string? GuiaRemision { get; set; }

    [XmlElement("razonSocialComprador")]
    public string RazonSocialComprador { get; set; } = string.Empty;

    [XmlElement("identificacionComprador")]
    public string IdentificacionComprador { get; set; } = string.Empty;

    [XmlElement("direccionComprador")]
    public string? DireccionComprador { get; set; }

    [XmlElement("totalSinImpuestos")]
    public decimal TotalSinImpuestos { get; set; }

    [XmlElement("totalDescuento")]
    public decimal TotalDescuento { get; set; }

    [XmlElement("totalConImpuestos")]
    public TotalConImpuestos TotalConImpuestos { get; set; } = new();

    [XmlElement("propina")]
    public decimal Propina { get; set; }

    [XmlElement("importeTotal")]
    public decimal ImporteTotal { get; set; }

    [XmlElement("moneda")]
    public string? Moneda { get; set; }

    [XmlElement("pagos")]
    public Pagos Pagos { get; set; } = new();

    [XmlElement("valorRetIva")]
    public decimal ValorRetIva { get; set; }

    [XmlElement("valorRetRenta")]
    public decimal ValorRetRenta { get; set; }
}

public class TotalConImpuestos
{
    [XmlElement("totalImpuesto")]
    public List<TotalImpuesto> TotalImpuesto { get; set; } = new();
}

public class TotalImpuesto
{
    [XmlElement("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [XmlElement("codigoPorcentaje")]
    public string CodigoPorcentaje { get; set; } = string.Empty;

    [XmlElement("descuentoAdicional")]
    public decimal? DescuentoAdicional { get; set; }

    [XmlElement("baseImponible")]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor")]
    public decimal Valor { get; set; }
}

public class Pagos
{
    [XmlElement("pago")]
    public List<Pago> Pago { get; set; } = new();
}

public class Pago
{
    [XmlElement("formaPago")]
    public string FormaPago { get; set; } = string.Empty;

    [XmlElement("total")]
    public decimal Total { get; set; }

    [XmlElement("plazo")]
    public int? Plazo { get; set; }

    [XmlElement("unidadTiempo")]
    public string? UnidadTiempo { get; set; }
}

public class Detalles
{
    [XmlElement("detalle")]
    public List<Detalle> Detalle { get; set; } = new();
}

public class Detalle
{
    [XmlElement("codigoPrincipal")]
    public string CodigoPrincipal { get; set; } = string.Empty;

    [XmlElement("codigoAuxiliar")]
    public string? CodigoAuxiliar { get; set; }

    [XmlElement("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    [XmlElement("cantidad")]
    public decimal Cantidad { get; set; }

    [XmlElement("precioUnitario")]
    public decimal PrecioUnitario { get; set; }

    [XmlElement("descuento")]
    public decimal Descuento { get; set; }

    [XmlElement("precioTotalSinImpuesto")]
    public decimal PrecioTotalSinImpuesto { get; set; }

    [XmlElement("detallesAdicionales")]
    public DetallesAdicionales? DetallesAdicionales { get; set; }

    [XmlElement("impuestos")]
    public ImpuestosDetalle Impuestos { get; set; } = new();
}

public class DetallesAdicionales
{
    [XmlElement("detAdicional")]
    public List<DetAdicional> DetAdicional { get; set; } = new();
}

public class DetAdicional
{
    [XmlAttribute("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [XmlAttribute("valor")]
    public string Valor { get; set; } = string.Empty;
}

public class ImpuestosDetalle
{
    [XmlElement("impuesto")]
    public List<ImpuestoDetalle> Impuesto { get; set; } = new();
}

public class ImpuestoDetalle
{
    [XmlElement("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [XmlElement("codigoPorcentaje")]
    public string CodigoPorcentaje { get; set; } = string.Empty;

    [XmlElement("tarifa")]
    public decimal Tarifa { get; set; }

    [XmlElement("baseImponible")]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor")]
    public decimal Valor { get; set; }
}

public class Retenciones
{
    [XmlElement("retencion")]
    public List<Retencion> Retencion { get; set; } = new();
}

public class Retencion
{
    [XmlElement("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [XmlElement("codigoPorcentaje")]
    public string CodigoPorcentaje { get; set; } = string.Empty;

    [XmlElement("tarifa")]
    public decimal Tarifa { get; set; }

    [XmlElement("valor")]
    public decimal Valor { get; set; }
}

public class InfoAdicional
{
    [XmlElement("campoAdicional")]
    public List<CampoAdicional> CampoAdicional { get; set; } = new();
}

public class CampoAdicional
{
    [XmlAttribute("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [XmlText]
    public string Valor { get; set; } = string.Empty;
}
