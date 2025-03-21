using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace QuestPDF.ZUGFeRD;

public class Tests
{
    [Test]
    public void ZUGFeRD_Test()
    {
        // TODO: Please make sure that you are eligible to use the Community license.
        // To learn more about the QuestPDF licensing, please visit:
        // https://www.questpdf.com/pricing.html
        QuestPDF.Settings.License = LicenseType.Community;
        
        Document
            .Create(document =>
            {
                document.Page(page =>
                {
                    page.Content().Text("Your invoice content");
                });
            })
            .WithSettings(new DocumentSettings { PdfA = true }) // PDF/A-3b
            .GeneratePdf("invoice.pdf");
        
        DocumentOperation
            .LoadFile("invoice.pdf")
            .AddAttachment(new DocumentOperation.DocumentAttachment
            {
                Key = "factur-zugferd",
                FilePath = "resource-factur-x.xml",
                AttachmentName = "factur-x.xml",
                MimeType = "text/xml",
                Description = "Factur-X Invoice",
                Relationship = DocumentOperation.DocumentAttachmentRelationship.Source,
                CreationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            })
            .ExtendMetadata(File.ReadAllText("resource-zugferd-metadata.xml"))
            .Save("zugferd-invoice.pdf");
    }
}