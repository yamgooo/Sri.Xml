# Servicio XML SRI

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-Yamgooo.SRI.Xml-blue.svg)](https://www.nuget.org/packages/Yamgooo.SRI.Xml)

Una librería .NET para generar y parsear XML de facturas electrónicas del SRI de Ecuador en el formato oficial aceptado por el SRI (Factura v1.1.0). Úsala para producir XML conforme para el envío y para mapear XML existentes del SRI a modelos fuertemente tipados.

Además de la serialización/deserialización de XML, este paquete expone modelos de dominio en C# que mapean de forma 1:1 los artefactos de la especificación del SRI (por ejemplo, `factura`, `infoTributaria`, `infoFactura`, `detalles`, `impuestos`). Estos modelos encapsulan los campos requeridos por el SRI, tipos de datos y reglas estructurales, permitiendo trabajar de forma robusta y tipada contra los esquemas normativos mientras aprovechas utilidades de validación y formateo conforme.

Nota: La firma digital (XAdES) está fuera del alcance de este paquete. Para firmar, utiliza `Yamgooo.SRI.Sign` junto con esta librería.

También disponible en inglés: [README.md](README.md)

## 🚀 Características

- **Generación de XML**: Serialización robusta con `XmlSerializer`, salida UTF-8 con indentación y espacios de nombres limpios
- **Deserialización**: Conversión segura de XML a modelos fuertemente tipados
- **Validación**: Validación estructural previa a la generación con mensajes detallados
- **Modelos SRI**: Modelos C# completos y fuertemente tipados que reflejan la `Factura v1.1.0` del SRI (p. ej., `SriInvoice`, `InfoTributaria`, `InfoFactura`, `Detalles`, impuestos, pagos). Diseñados para reflejar la estructura y restricciones oficiales del XML
- **Async**: Métodos asíncronos para operaciones no bloqueantes
- **Logging**: Integración con `Microsoft.Extensions.Logging`
- **Manejo de errores**: Excepciones claras y trazas útiles

## 📦 Instalación

### Paquete NuGet
```bash
dotnet add package Yamgooo.SRI.Xml
```

### Instalación manual
```bash
git clone https://github.com/yamgooo/Sri.Xml.git
cd Sri.Xml
dotnet build
```

## 🛠️ Inicio rápido

### 1) Registrar el servicio (DI)

```csharp
using Microsoft.Extensions.DependencyInjection;
using Yamgooo.SRI.Xml;

var services = new ServiceCollection();

// Registrar el servicio
services.AddLogging();
services.AddSriXmlService();

var provider = services.BuildServiceProvider();
var sriXml = provider.GetRequiredService<ISriXmlService>();
```

### 2) Generar XML de factura

```csharp
using Yamgooo.SRI.Xml.Models;

var invoice = new SriInvoice
{
    InfoTributaria = new InfoTributaria
    {
        Ambiente = "2",                // 1=Pruebas, 2=Producción
        TipoEmision = "1",             // 1=Normal
        RazonSocial = "Mi Empresa S.A.",
        Ruc = "1790012345001",
        ClaveAcceso = "1234567890123456789012345678901234567890123456789",
        Estab = "001",
        PtoEmi = "001",
        Secuencial = "000000123",
        DirMatriz = "Calle Falsa 123"
    },
    InfoFactura = new InfoFactura
    {
        FechaEmision = "09/01/2025",
        TipoIdentificacionComprador = "05", // Cédula
        RazonSocialComprador = "Juan Pérez",
        IdentificacionComprador = "0912345678",
        TotalSinImpuestos = 100.00m,
        ImporteTotal = 112.00m,
        TotalConImpuestos = new TotalConImpuestos
        {
            TotalImpuesto =
            {
                new TotalImpuesto
                {
                    Codigo = "2",            // IVA
                    CodigoPorcentaje = "2",  // 12%
                    BaseImponible = 100.00m,
                    Valor = 12.00m
                }
            }
        },
        Pagos = new Pagos
        {
            Pago = { new Pago { FormaPago = "01", Total = 112.00m } }
        }
    },
    Detalles = new Detalles
    {
        Detalle =
        {
            new Detalle
            {
                CodigoPrincipal = "SKU-001",
                Descripcion = "Producto de ejemplo",
                Cantidad = 1,
                PrecioUnitario = 100.00m,
                Descuento = 0,
                PrecioTotalSinImpuesto = 100.00m,
                Impuestos = new ImpuestosDetalle
                {
                    Impuesto =
                    {
                        new ImpuestoDetalle
                        {
                            Codigo = "2",
                            CodigoPorcentaje = "2",
                            Tarifa = 12.00m,
                            BaseImponible = 100.00m,
                            Valor = 12.00m
                        }
                    }
                }
            }
        }
    }
};

var xml = await sriXml.GenerateInvoiceXmlAsync(invoice);
```

### 3) Deserializar XML de factura

```csharp
var invoiceModel = await sriXml.DeserializeInvoiceXmlAsync(xml);
```

### 4) Validar estructura antes de generar

```csharp
var validation = sriXml.ValidateInvoiceStructure(invoice);
if (!validation.IsValid)
{
    Console.WriteLine($"Errores: {validation.ErrorMessage}");
}
```

## 📋 Referencia de API

### Interfaz `ISriXmlService`

```csharp
Task<string> GenerateInvoiceXmlAsync(SriInvoice invoice);
Task<SriInvoice> DeserializeInvoiceXmlAsync(string xmlContent);
ValidationResult ValidateInvoiceStructure(SriInvoice invoice);
string GenerateAccessKeyFromSriInvoice(SriInvoice invoice);
```

### Clases principales (`Yamgooo.SRI.Xml.Models`)

- `SriInvoice` (raíz `factura` v1.1.0)
- `InfoTributaria`
- `InfoFactura`
- `Detalles` / `Detalle`
- `ImpuestosDetalle` / `ImpuestoDetalle`
- `TotalConImpuestos` / `TotalImpuesto`
- `Pagos` / `Pago`
- `InfoAdicional`, `Retenciones` (opcionales)

## 🔧 Reglas de validación (resumen)

- **`InfoTributaria`**: requeridos `Ambiente`, `TipoEmision`, `RazonSocial`, `Ruc`, `ClaveAcceso`, `Estab`, `PtoEmi`, `Secuencial`, `DirMatriz`.
- **`InfoFactura`**: requeridos `FechaEmision`, `TipoIdentificacionComprador`, `RazonSocialComprador`, `IdentificacionComprador`. Valores > 0 en `TotalSinImpuestos` e `ImporteTotal`. Al menos un `Pago`.
- **`Detalles`**: al menos un `Detalle` con `CodigoPrincipal`, `Descripcion`; `Cantidad`, `PrecioUnitario` y `PrecioTotalSinImpuesto` > 0; al menos un impuesto en `Impuestos`.

Los errores se exponen vía `ValidationResult.Errors` y `ValidationResult.ErrorMessage`.

## 🧪 Pruebas

```csharp
using Microsoft.Extensions.Logging;
using Moq;
using Yamgooo.SRI.Xml;

[Test]
public async Task GenerateInvoiceXmlAsync_MinValid_ReturnsXml()
{
    var mockLogger = new Mock<ILogger<SriXmlService>>();
    var service = new SriXmlService(mockLogger.Object);

    var invoice = /* construir un objeto válido similar al ejemplo */;

    var xml = await service.GenerateInvoiceXmlAsync(invoice);

    Assert.IsFalse(string.IsNullOrWhiteSpace(xml));
}
```

## 🚀 Rendimiento

- Operaciones de E/S asíncronas
- Escritura eficiente con `XmlWriter` y `StringWriter`
- Salida UTF-8 con indentación

## 📦 Dependencias

- **.NET 8.0** (TargetFramework)
- **Microsoft.Extensions.Logging.Abstractions** (logging)

## 🔒 Consideraciones

- Valida reglas de negocio y formatos del SRI antes del envío
- Evita registrar datos sensibles (RUC, claves de acceso)

## 🤝 Contribución

1. Haz fork
2. Crea una rama de feature (`git checkout -b feature/mi-feature-increible`)
3. Commits (`git commit -m "feat: mi feature increible"`)
4. Push (`git push origin feature/mi-feature-increible`)
5. Abre un Pull Request

## 📄 Licencia

MIT. Ver [LICENSE](LICENSE).

## 📞 Soporte

- Issues: `https://github.com/yamgooo/Sri.Xml/issues`
- Documentación: `https://github.com/yamgooo`
- Email: erikportillapesantez@outlook.com

---

Hecho con ❤️ para la comunidad de desarrolladores ecuatoriana


