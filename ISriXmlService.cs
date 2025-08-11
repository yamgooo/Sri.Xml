using Yamgooo.SRI.Xml.Models;

namespace Yamgooo.SRI.Xml;


/// <summary>
/// Interface for SRI XML operations including serialization and deserialization
/// </summary>
public interface ISriXmlService
{
    /// <summary>
    /// Generates SRI invoice XML from model
    /// </summary>
    /// <param name="invoice">SRI invoice model</param>
    /// <returns>XML string</returns>
    Task<string> GenerateInvoiceXmlAsync(SriInvoice invoice);
    
    /// <summary>
    /// Deserializes XML string to SRI invoice model
    /// </summary>
    /// <param name="xmlContent">XML content as string</param>
    /// <returns>SRI invoice model</returns>
    Task<SriInvoice> DeserializeInvoiceXmlAsync(string xmlContent);
    
    /// <summary>
    /// Validates invoice structure before XML generation
    /// </summary>
    /// <param name="invoice">Invoice model to validate</param>
    /// <returns>Validation result with errors if any</returns>
    ValidationResult ValidateInvoiceStructure(SriInvoice invoice);
    
    /// <summary>
    /// Generates the SRI "Clave de Acceso" from a populated <see cref="SriInvoice"/> model.
    /// Uses the official Mod-11 algorithm and concatenates the fields as per SRI specification
    /// (fechaEmision ddMMyyyy + codDoc + ruc + ambiente + estab + ptoEmi + secuencial + codigoNumerico(8) + tipoEmision + digitoVerificador).
    /// </summary>
    /// <param name="invoice">The invoice source model</param>
    /// <returns>The 49-digit access key, or empty string if it cannot be generated</returns>
    string GenerateAccessKeyFromSriInvoice(SriInvoice invoice);
    
}