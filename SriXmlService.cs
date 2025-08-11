// By: Erik Portilla
// Date: 2025-08-10
// -------------------------------------------------------------

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Yamgooo.SRI.Xml.Models;
using System.Globalization;

namespace Yamgooo.SRI.Xml;


/// <summary>
/// Professional SRI XML Service for Ecuadorian Electronic Invoicing
/// Handles XML generation, deserialization, and validation
/// </summary>
public class SriXmlService(ILogger<SriXmlService> logger) : ISriXmlService
{
    private readonly ILogger<SriXmlService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly XmlSerializer _serializer = new(typeof(SriInvoice));

    /// <summary>
    /// Generates SRI invoice XML using professional XML serialization
    /// </summary>
    /// <param name="invoice">Complete SRI invoice model</param>
    /// <returns>Serialized XML as string</returns>
    public async Task<string> GenerateInvoiceXmlAsync(SriInvoice invoice)
    {
        try
        {
            _logger.LogInformation("Starting XML generation for invoice {Sequential}", 
                invoice?.InfoTributaria?.Secuencial ?? "N/A");

            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null");

            // Auto-generate access key if missing
            if (string.IsNullOrWhiteSpace(invoice.InfoTributaria?.ClaveAcceso))
            {
                var generatedKey = GenerateAccessKeyFromSriInvoice(invoice);
                if (!string.IsNullOrWhiteSpace(generatedKey))
                {
                    invoice.InfoTributaria.ClaveAcceso = generatedKey;
                    _logger.LogInformation("Access key generated automatically");
                }
            }

            // Validate invoice structure
            var validationResult = ValidateInvoiceStructure(invoice);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {ValidationErrors}", validationResult.ErrorMessage);
                throw new InvalidOperationException($"Invalid invoice structure: {validationResult.ErrorMessage}");
            }

            // Configure XML writer settings for professional output
            var settings = new XmlWriterSettings
            {
                Async = true,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = true 
            };

            await using var stringWriter = new StringWriter();
            await using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            
            // Serialize the invoice
            _serializer.Serialize(xmlWriter, invoice);
            
            var xmlResult = stringWriter.ToString();
            
            _logger.LogInformation("XML generated successfully for invoice {Sequential}. Size: {Size} bytes", 
                invoice.InfoTributaria.Secuencial, xmlResult.Length);

            return await Task.FromResult(xmlResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating XML for invoice {Sequential}", 
                invoice?.InfoTributaria?.Secuencial ?? "N/A");
            throw;
        }
    }

    /// <summary>
    /// Deserializes XML string to SRI invoice model
    /// </summary>
    /// <param name="xmlContent">XML content as string</param>
    /// <returns>SRI invoice model</returns>
    public async Task<SriInvoice> DeserializeInvoiceXmlAsync(string xmlContent)
    {
        try
        {
            _logger.LogInformation("Starting XML deserialization");

            if (string.IsNullOrWhiteSpace(xmlContent))
            {
                throw new ArgumentException("XML content cannot be empty or null", nameof(xmlContent));
            }

            // Validate XML structure
            if (!IsValidXml(xmlContent))
            {
                throw new InvalidOperationException("Invalid XML structure");
            }

            using var stringReader = new StringReader(xmlContent);
            using var xmlReader = XmlReader.Create(stringReader);
            
            var invoice = (SriInvoice)_serializer.Deserialize(xmlReader);
            
            if (invoice == null)
            {
                throw new InvalidOperationException("Failed to deserialize XML to invoice model");
            }

            _logger.LogInformation("XML deserialized successfully for invoice {Sequential}", 
                invoice.InfoTributaria?.Secuencial ?? "N/A");

            return await Task.FromResult(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing XML invoice");
            throw;
        }
    }

    /// <summary>
    /// Validates invoice structure before XML generation
    /// </summary>
    /// <param name="invoice">Invoice model to validate</param>
    /// <returns>Validation result with errors if any</returns>
    public ValidationResult ValidateInvoiceStructure(SriInvoice invoice)
    {
        var result = new ValidationResult { IsValid = true };

        if (invoice == null)
        {
            result.IsValid = false;
            result.Errors.Add("Invoice cannot be null");
            return result;
        }

        // Validate InfoTributaria
        ValidateInfoTributaria(invoice.InfoTributaria, result);

        // Validate InfoFactura
        ValidateInfoFactura(invoice.InfoFactura, result);

        // Validate Detalles
        ValidateDetalles(invoice.Detalles, result);

        return result;
    }
    
    #region Private Methods

    /// <summary>
    /// Calculates SRI Mod-11 check digit according to official specification.
    /// Returns 0 when result is 11 and 1 when result is 10.
    /// </summary>
    private static int CalculateMod11(string input)
    {
        const int baseValue = 11;
        const int minWeight = 2;
        const int maxWeight = 7;

        int total = 0;
        int weight = minWeight;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            int digit = input[i] - '0';
            total += digit * weight;
            weight++;
            if (weight > maxWeight)
            {
                weight = minWeight;
            }
        }

        int mod = baseValue - (total % baseValue);
        if (mod == baseValue) return 0; // 11 -> 0
        if (mod == baseValue - 1) return 1; // 10 -> 1
        return mod;
    }

    /// <summary>
    /// Generates an 8-digit numeric security code. In production, consider using a deterministic source if needed.
    /// </summary>
    private static string GenerateEightDigitSecurityCode()
    {
        var random = new Random();
        return random.Next(10_000_000, 99_999_999).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Validates XML structure
    /// </summary>
    /// <param name="xmlContent">XML content to validate</param>
    /// <returns>True if valid XML</returns>
    private bool IsValidXml(string xmlContent)
    {
        try
        {
            using var stringReader = new StringReader(xmlContent);
            using var xmlReader = XmlReader.Create(stringReader);
            
            while (xmlReader.Read())
            {
                // Just read through the XML to validate structure
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates InfoTributaria section
    /// </summary>
    /// <param name="infoTributaria">InfoTributaria to validate</param>
    /// <param name="result">Validation result to update</param>
    private void ValidateInfoTributaria(InfoTributaria infoTributaria, ValidationResult result)
    {
        if (infoTributaria == null)
        {
            result.IsValid = false;
            result.Errors.Add("InfoTributaria is required");
            return;
        }

        var requiredFields = new Dictionary<string, string>
        {
            { nameof(infoTributaria.Ambiente), infoTributaria.Ambiente },
            { nameof(infoTributaria.TipoEmision), infoTributaria.TipoEmision },
            { nameof(infoTributaria.RazonSocial), infoTributaria.RazonSocial },
            { nameof(infoTributaria.Ruc), infoTributaria.Ruc },
            { nameof(infoTributaria.ClaveAcceso), infoTributaria.ClaveAcceso },
            { nameof(infoTributaria.Estab), infoTributaria.Estab },
            { nameof(infoTributaria.PtoEmi), infoTributaria.PtoEmi },
            { nameof(infoTributaria.Secuencial), infoTributaria.Secuencial },
            { nameof(infoTributaria.DirMatriz), infoTributaria.DirMatriz }
        };

        foreach (var field in requiredFields)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                result.IsValid = false;
                result.Errors.Add($"{field.Key} is required");
            }
        }
    }

    /// <summary>
    /// Validates InfoFactura section
    /// </summary>
    /// <param name="infoFactura">InfoFactura to validate</param>
    /// <param name="result">Validation result to update</param>
    private void ValidateInfoFactura(InfoFactura infoFactura, ValidationResult result)
    {
        if (infoFactura == null)
        {
            result.IsValid = false;
            result.Errors.Add("InfoFactura is required");
            return;
        }

        var requiredFields = new Dictionary<string, string>
        {
            { nameof(infoFactura.FechaEmision), infoFactura.FechaEmision },
            { nameof(infoFactura.TipoIdentificacionComprador), infoFactura.TipoIdentificacionComprador },
            { nameof(infoFactura.RazonSocialComprador), infoFactura.RazonSocialComprador },
            { nameof(infoFactura.IdentificacionComprador), infoFactura.IdentificacionComprador }
        };

        foreach (var field in requiredFields)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                result.IsValid = false;
                result.Errors.Add($"{field.Key} is required");
            }
        }

        if (infoFactura.TotalSinImpuestos <= 0)
        {
            result.IsValid = false;
            result.Errors.Add("TotalSinImpuestos must be greater than 0");
        }

        if (infoFactura.ImporteTotal <= 0)
        {
            result.IsValid = false;
            result.Errors.Add("ImporteTotal must be greater than 0");
        }

        if (infoFactura.Pagos?.Pago == null || !infoFactura.Pagos.Pago.Any())
        {
            result.IsValid = false;
            result.Errors.Add("At least one payment is required");
        }
    }

    /// <summary>
    /// Validates Detalles section
    /// </summary>
    /// <param name="detalles">Detalles to validate</param>
    /// <param name="result">Validation result to update</param>
    private void ValidateDetalles(Detalles detalles, ValidationResult result)
    {
        if (detalles?.Detalle == null || !detalles.Detalle.Any())
        {
            result.IsValid = false;
            result.Errors.Add("At least one detail item is required");
            return;
        }

        for (int i = 0; i < detalles.Detalle.Count; i++)
        {
            var detalle = detalles.Detalle[i];
            var detailIndex = i + 1;

            ValidateDetalleItem(detalle, detailIndex, result);
        }
    }

    /// <summary>
    /// Validates individual detail item
    /// </summary>
    /// <param name="detalle">Detail item to validate</param>
    /// <param name="detailIndex">Index of the detail item</param>
    /// <param name="result">Validation result to update</param>
    private void ValidateDetalleItem(Detalle detalle, int detailIndex, ValidationResult result)
    {
        var requiredFields = new Dictionary<string, string>
        {
            { "CodigoPrincipal", detalle.CodigoPrincipal },
            { "Descripcion", detalle.Descripcion }
        };

        foreach (var field in requiredFields)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                result.IsValid = false;
                result.Errors.Add($"Detail {detailIndex}: {field.Key} is required");
            }
        }

        if (detalle.Cantidad <= 0)
        {
            result.IsValid = false;
            result.Errors.Add($"Detail {detailIndex}: Quantity must be greater than 0");
        }

        if (detalle.PrecioUnitario <= 0)
        {
            result.IsValid = false;
            result.Errors.Add($"Detail {detailIndex}: UnitPrice must be greater than 0");
        }

        if (detalle.PrecioTotalSinImpuesto <= 0)
        {
            result.IsValid = false;
            result.Errors.Add($"Detail {detailIndex}: TotalPriceWithoutTax must be greater than 0");
        }

        if (detalle.Impuestos?.Impuesto == null || !detalle.Impuestos.Impuesto.Any())
        {
            result.IsValid = false;
            result.Errors.Add($"Detail {detailIndex}: At least one tax is required");
        }
    }

    #endregion

    /// <summary>
    /// Generates the SRI Access Key (Clave de Acceso) from a populated SriInvoice model.
    /// Format: ddMMyyyy + codDoc + ruc + ambiente + estab + ptoEmi + secuencial + codigoNumerico(8) + tipoEmision + digitoVerificador
    /// </summary>
    public string GenerateAccessKeyFromSriInvoice(SriInvoice sriInvoice)
    {
        try
        {
            if (sriInvoice?.InfoTributaria == null || sriInvoice.InfoFactura == null)
            {
                return string.Empty;
            }

            var ruc = sriInvoice.InfoTributaria.Ruc;
            var ambiente = sriInvoice.InfoTributaria.Ambiente;
            var establecimiento = sriInvoice.InfoTributaria.Estab;
            var puntoEmision = sriInvoice.InfoTributaria.PtoEmi;
            var secuencial = sriInvoice.InfoTributaria.Secuencial?.Replace("-", string.Empty);
            var tipoDocumento = sriInvoice.InfoTributaria.CodDoc;
            var fechaEmision = sriInvoice.InfoFactura.FechaEmision;
            var tipoEmision = sriInvoice.InfoTributaria.TipoEmision;

            if (string.IsNullOrWhiteSpace(ruc) || string.IsNullOrWhiteSpace(ambiente) ||
                string.IsNullOrWhiteSpace(establecimiento) || string.IsNullOrWhiteSpace(puntoEmision) ||
                string.IsNullOrWhiteSpace(secuencial) || string.IsNullOrWhiteSpace(tipoDocumento) ||
                string.IsNullOrWhiteSpace(fechaEmision) || string.IsNullOrWhiteSpace(tipoEmision))
            {
                return string.Empty;
            }

            // Date dd/MM/yyyy -> ddMMyyyy
            var date = DateTime.ParseExact(fechaEmision, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var fecha = date.ToString("ddMMyyyy", CultureInfo.InvariantCulture);

            var codigoNumerico = GenerateEightDigitSecurityCode();

            var body = string.Concat(
                fecha,
                tipoDocumento,
                ruc,
                ambiente,
                establecimiento,
                puntoEmision,
                secuencial,
                codigoNumerico,
                tipoEmision
            );

            var dv = CalculateMod11(body);
            var claveAcceso = body + dv.ToString(CultureInfo.InvariantCulture);

            return claveAcceso;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating access key from SriInvoice");
            return string.Empty;
        }
    }
}