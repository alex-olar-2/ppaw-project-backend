using Azure;
using Azure.AI.DocumentIntelligence;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.Extensions.Configuration;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExtractInfoIdentityDocument.Services
{
    public class IdentityDocumentAnalyzerService : IIdentityDocumentAnalyzerService
    {
        private readonly DocumentIntelligenceClient _client;
        private readonly IConfiguration _configuration;

        public IdentityDocumentAnalyzerService(IConfiguration configuration)
        {
            _configuration = configuration;

            string endpoint = _configuration["AzureDocumentIntelligence:Endpoint"];
            string key = _configuration["AzureDocumentIntelligence:Key"];

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Azure Document Intelligence endpoint and key must be configured in appsettings.json");
            }

            AzureKeyCredential credential = new AzureKeyCredential(key);
            _client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
        }

        public async Task<IdentityDocumentResult> AnalyzeIdentityDocumentFromUrlAsync(string documentUrl)
        {
            Uri idDocumentUri = new Uri(documentUrl);

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-idDocument",
                idDocumentUri);

            AnalyzeResult identityDocuments = operation.Value;

            return ExtractIdentityInformation(identityDocuments);
        }

        public async Task<IdentityDocumentResult> AnalyzeIdentityDocumentFromStreamAsync(Stream documentStream)
        {
            using var memoryStream = new MemoryStream();
            await documentStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            BinaryData documentData = BinaryData.FromBytes(memoryStream.ToArray());

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-idDocument",
                documentData);

            AnalyzeResult identityDocuments = operation.Value;

            return ExtractIdentityInformation(identityDocuments);
        }

        private IdentityDocumentResult ExtractIdentityInformation(AnalyzeResult analyzeResult)
        {
            if (analyzeResult.Documents == null || !analyzeResult.Documents.Any())
            {
                throw new InvalidOperationException("No identity document found in the analyzed result");
            }

            AnalyzedDocument identityDocument = analyzeResult.Documents.First();
            var result = new IdentityDocumentResult();

            // Extract Address
            if (identityDocument.Fields.TryGetValue("Address", out DocumentField addressField)
                && addressField.FieldType == DocumentFieldType.Address)
            {
                result.Address = addressField.Content;
                result.AddressConfidence = addressField.Confidence;
            }

            // Extract Country/Region
            if (identityDocument.Fields.TryGetValue("CountryRegion", out DocumentField countryRegionField)
                && countryRegionField.FieldType == DocumentFieldType.CountryRegion)
            {
                result.CountryRegion = countryRegionField.ValueCountryRegion;
                result.CountryRegionConfidence = countryRegionField.Confidence;
            }

            // Extract Date of Birth
            if (identityDocument.Fields.TryGetValue("DateOfBirth", out DocumentField dateOfBirthField)
                && dateOfBirthField.FieldType == DocumentFieldType.Date)
            {
                result.DateOfBirth = dateOfBirthField.ValueDate;
                result.DateOfBirthConfidence = dateOfBirthField.Confidence;
            }

            // Extract Date of Expiration
            if (identityDocument.Fields.TryGetValue("DateOfExpiration", out DocumentField dateOfExpirationField)
                && dateOfExpirationField.FieldType == DocumentFieldType.Date)
            {
                result.DateOfExpiration = dateOfExpirationField.ValueDate;
                result.DateOfExpirationConfidence = dateOfExpirationField.Confidence;
            }

            // Extract Date of Issue
            if (identityDocument.Fields.TryGetValue("DateOfIssue", out DocumentField dateOfIssueField)
                && dateOfIssueField.FieldType == DocumentFieldType.Date)
            {
                result.DateOfIssue = dateOfIssueField.ValueDate;
                result.DateOfIssueConfidence = dateOfIssueField.Confidence;
            }

            // Extract Document Number
            if (identityDocument.Fields.TryGetValue("DocumentNumber", out DocumentField documentNumberField)
                && documentNumberField.FieldType == DocumentFieldType.String)
            {
                result.DocumentNumber = documentNumberField.ValueString;
                result.DocumentNumberConfidence = documentNumberField.Confidence;
            }

            // Extract First Name
            if (identityDocument.Fields.TryGetValue("FirstName", out DocumentField firstNameField)
                && firstNameField.FieldType == DocumentFieldType.String)
            {
                result.FirstName = firstNameField.ValueString;
                result.FirstNameConfidence = firstNameField.Confidence;
            }

            // Extract Last Name
            if (identityDocument.Fields.TryGetValue("LastName", out DocumentField lastNameField)
                && lastNameField.FieldType == DocumentFieldType.String)
            {
                result.LastName = lastNameField.ValueString;
                result.LastNameConfidence = lastNameField.Confidence;
            }

            // Extract Region
            if (identityDocument.Fields.TryGetValue("Region", out DocumentField regionField)
                && regionField.FieldType == DocumentFieldType.String)
            {
                result.Region = regionField.ValueString;
                result.RegionConfidence = regionField.Confidence;
            }

            // Extract Sex
            if (identityDocument.Fields.TryGetValue("Sex", out DocumentField sexField)
                && sexField.FieldType == DocumentFieldType.String)
            {
                result.Sex = sexField.ValueString;
                result.SexConfidence = sexField.Confidence;
            }

            // Extract Nationality
            if (identityDocument.Fields.TryGetValue("Nationality", out DocumentField nationalityField)
                && nationalityField.FieldType == DocumentFieldType.String)
            {
                result.Nationality = nationalityField.ValueString;
                result.NationalityConfidence = nationalityField.Confidence;
            }

            // Extract Document Type
            if (identityDocument.Fields.TryGetValue("DocumentType", out DocumentField documentTypeField)
                && documentTypeField.FieldType == DocumentFieldType.String)
            {
                result.DocumentType = documentTypeField.ValueString;
                result.DocumentTypeConfidence = documentTypeField.Confidence;
            }

            return result;
        }
    }
}
