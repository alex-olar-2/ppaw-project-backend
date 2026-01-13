namespace ExtractInfoIdentityDocument.Models
{
    public class IdentityDocumentResult
    {
        public string FirstName { get; set; }
        public float? FirstNameConfidence { get; set; }

        public string LastName { get; set; }
        public float? LastNameConfidence { get; set; }

        public string DocumentNumber { get; set; }
        public float? DocumentNumberConfidence { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }
        public float? DateOfBirthConfidence { get; set; }

        public DateTimeOffset? DateOfExpiration { get; set; }
        public float? DateOfExpirationConfidence { get; set; }

        public DateTimeOffset? DateOfIssue { get; set; }
        public float? DateOfIssueConfidence { get; set; }

        public string Sex { get; set; }
        public float? SexConfidence { get; set; }

        public string Address { get; set; }
        public float? AddressConfidence { get; set; }

        public string CountryRegion { get; set; }
        public float? CountryRegionConfidence { get; set; }

        public string Region { get; set; }
        public float? RegionConfidence { get; set; }

        public string Nationality { get; set; }
        public float? NationalityConfidence { get; set; }

        public string DocumentType { get; set; }
        public float? DocumentTypeConfidence { get; set; }
    }
}
