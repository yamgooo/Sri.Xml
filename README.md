# SRI XML Service

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-Yamgooo.SRI.Xml-blue.svg)](https://www.nuget.org/packages/Yamgooo.SRI.Xml)

A professional .NET library to generate, deserialize, and validate SRI (Ecuadorian tax authority) electronic invoice XML, version 1.1.0.

## 🚀 Features

- **XML Generation**: Robust serialization with `XmlSerializer`, formatted UTF-8 output with indentation and clean namespaces
- **Deserialization**: Safe conversion from XML to strongly-typed models
- **Validation**: Pre-generation structural validation with detailed messages
- **SRI Models**: Complete invoice models for version `1.1.0` (`SriInvoice`, `InfoTributaria`, `InfoFactura`, `Detalles`, etc.)
- **Async**: Async methods for non-blocking operations
- **Logging**: Integration with `Microsoft.Extensions.Logging`
- **Error Handling**: Clear exceptions and helpful traces

## 📦 Installation

### NuGet Package
```bash
dotnet add package Yamgooo.SRI.Xml
```

### Manual Installation
```bash
git clone https://github.com/yamgooo/Sri.Xml.git
cd Sri.Xml
dotnet build
```

## 🛠️ Quick Start

### 1) Register the service (DI)

```csharp
using Microsoft.Extensions.DependencyInjection;
using Yamgooo.SRI.Xml;

var services = new ServiceCollection();

// Register the service
services.AddLogging();
services.AddSingleton<ISriXmlService, SriXmlService>();

var provider = services.BuildServiceProvider();
var sriXml = provider.GetRequiredService<ISriXmlService>();
```

### 2) Generate invoice XML

```csharp
using Yamgooo.SRI.Xml.Models;

var invoice = new SriInvoice
{
    InfoTributaria = new InfoTributaria
    {
        Ambiente = "2",                // 1=Testing, 2=Production
        TipoEmision = "1",             // 1=Normal
        RazonSocial = "My Company S.A.",
        Ruc = "1790012345001",
        ClaveAcceso = "1234567890123456789012345678901234567890123456789",
        Estab = "001",
        PtoEmi = "001",
        Secuencial = "000000123",
        DirMatriz = "123 Evergreen Ave"
    },
    InfoFactura = new InfoFactura
    {
        FechaEmision = "09/01/2025",
        TipoIdentificacionComprador = "05", // National ID
        RazonSocialComprador = "John Doe",
        IdentificacionComprador = "0912345678",
        TotalSinImpuestos = 100.00m,
        ImporteTotal = 112.00m,
        TotalConImpuestos = new TotalConImpuestos
        {
            TotalImpuesto =
            {
                new TotalImpuesto
                {
                    Codigo = "2",            // VAT
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
                Descripcion = "Sample product",
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

### 3) Deserialize invoice XML

```csharp
var invoiceModel = await sriXml.DeserializeInvoiceXmlAsync(xml);
```

### 4) Validate structure before generating

```csharp
var validation = sriXml.ValidateInvoiceStructure(invoice);
if (!validation.IsValid)
{
    Console.WriteLine($"Errors: {validation.ErrorMessage}");
}
```

## 📋 API Reference

### `ISriXmlService` Interface

```csharp
Task<string> GenerateInvoiceXmlAsync(SriInvoice invoice);
Task<SriInvoice> DeserializeInvoiceXmlAsync(string xmlContent);
ValidationResult ValidateInvoiceStructure(SriInvoice invoice);
```

### Main model classes (`Yamgooo.SRI.Xml.Models`)

- `SriInvoice` (root `factura` v1.1.0)
- `InfoTributaria`
- `InfoFactura`
- `Detalles` / `Detalle`
- `ImpuestosDetalle` / `ImpuestoDetalle`
- `TotalConImpuestos` / `TotalImpuesto`
- `Pagos` / `Pago`
- `InfoAdicional`, `Retenciones` (optional)

## 🔧 Validation rules (summary)

- `InfoTributaria`: required `Ambiente`, `TipoEmision`, `RazonSocial`, `Ruc`, `ClaveAcceso`, `Estab`, `PtoEmi`, `Secuencial`, `DirMatriz`.
- `InfoFactura`: required `FechaEmision`, `TipoIdentificacionComprador`, `RazonSocialComprador`, `IdentificacionComprador`. Values > 0 for `TotalSinImpuestos` and `ImporteTotal`. At least one `Pago`.
- `Detalles`: at least one `Detalle` with `CodigoPrincipal`, `Descripcion`; `Cantidad`, `PrecioUnitario` and `PrecioTotalSinImpuesto` > 0; at least one tax in `Impuestos`.

Errors are exposed via `ValidationResult.Errors` and `ValidationResult.ErrorMessage`.

## 🧪 Testing

```csharp
using Microsoft.Extensions.Logging;
using Moq;
using Yamgooo.SRI.Xml;

[Test]
public async Task GenerateInvoiceXmlAsync_MinValid_ReturnsXml()
{
    var mockLogger = new Mock<ILogger<SriXmlService>>();
    var service = new SriXmlService(mockLogger.Object);

    var invoice = /* build a valid object like the example */;

    var xml = await service.GenerateInvoiceXmlAsync(invoice);

    Assert.IsFalse(string.IsNullOrWhiteSpace(xml));
}
```

## 🚀 Performance

- Async I/O operations
- Efficient writing with `XmlWriter` and `StringWriter`
- UTF-8 output with indentation

## 📦 Dependencies

- **.NET 8.0** (TargetFramework)
- **Microsoft.Extensions.Logging.Abstractions** (logging)

## 🔒 Considerations

- Validate business rules and SRI formats before submission
- Avoid logging sensitive data (RUC, access keys)

## 🤝 Contributing

1. Fork
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit (`git commit -m "feat: amazing feature"`)
4. Push (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

MIT. See [LICENSE](LICENSE).

## 📞 Support

- Issues: `https://github.com/yamgooo/Sri.Xml/issues`
- Docs: `https://github.com/yamgooo`
- Email: erikportillapesantez@outlook.com

---

Made with ❤️ for the Ecuadorian developer community


