using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Mvc;

namespace ExtractInfoIdentityDocument.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityDocumentController : ControllerBase
    {
        private readonly IIdentityDocumentAnalyzerService _analyzerService;

        public IdentityDocumentController(IIdentityDocumentAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        /// <summary>
        /// Analyze identity document from URL
        /// </summary>
        [HttpPost("analyze-url")]
        public async Task<IActionResult> AnalyzeDocumentFromUrl([FromBody] AnalyzeDocumentUrlRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DocumentUrl))
                {
                    return BadRequest(new { message = "Document URL is required" });
                }

                var result = await _analyzerService.AnalyzeIdentityDocumentFromUrlAsync(request.DocumentUrl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error analyzing document", error = ex.Message });
            }
        }

        /// <summary>
        /// Analyze identity document from uploaded file
        /// </summary>
        [HttpPost("analyze-file")]
        public async Task<IActionResult> AnalyzeDocumentFromFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "File is required" });
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".bmp", ".tiff" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type. Allowed types: jpg, jpeg, png, pdf, bmp, tiff" });
                }

                using var stream = file.OpenReadStream();
                var result = await _analyzerService.AnalyzeIdentityDocumentFromStreamAsync(stream);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error analyzing document", error = ex.Message });
            }
        }

        /// <summary>
        /// Get supported fields for identity documents
        /// </summary>
        [HttpGet("supported-fields")]
        public IActionResult GetSupportedFields()
        {
            var fields = new
            {
                fields = new[]
                {
                    "Address",
                    "CountryRegion",
                    "DateOfBirth",
                    "DateOfExpiration",
                    "DateOfIssue",
                    "DocumentNumber",
                    "FirstName",
                    "LastName",
                    "Nationality",
                    "Sex",
                    "Region",
                    "DocumentType"
                }
            };
            return Ok(fields);
        }
    }

    public class AnalyzeDocumentUrlRequest
    {
        public string DocumentUrl { get; set; }
    }
}
