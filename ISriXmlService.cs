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
    
}