// -------------------------------------------------------------
// By: Erik Portilla
// Date: 2025-08-10
// -------------------------------------------------------------

namespace Yamgooo.SRI.Xml.Models;

/// <summary>
/// Validation result containing success status and error messages
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public string ErrorMessage => string.Join("; ", Errors);
}