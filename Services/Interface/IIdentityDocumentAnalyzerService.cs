using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IIdentityDocumentAnalyzerService
    {
        /// <summary>
        /// Analyzes an identity document from a URL
        /// </summary>
        /// <param name="documentUrl">URL of the identity document</param>
        /// <returns>Extracted identity document information</returns>
        Task<IdentityDocumentResult> AnalyzeIdentityDocumentFromUrlAsync(string documentUrl);

        /// <summary>
        /// Analyzes an identity document from a stream
        /// </summary>
        /// <param name="documentStream">Stream containing the identity document</param>
        /// <returns>Extracted identity document information</returns>
        Task<IdentityDocumentResult> AnalyzeIdentityDocumentFromStreamAsync(Stream documentStream);
    }
}
