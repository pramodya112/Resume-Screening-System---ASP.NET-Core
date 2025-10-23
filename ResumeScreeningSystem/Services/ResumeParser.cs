using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Reflection.PortableExecutable;
using System.Text;
using System.IO;

namespace ResumeScreeningSystem.Services
{
    public class ResumeParser
    {
        public static string ExtractText(string filepath)

        {
            if (System.IO.Path.GetExtension(filepath).ToLower() != ".pdf")
            {
                return "Unsupported file format. Please upload a PDF file.";
            }

            var text = new StringBuilder();
            using var reader = new PdfReader(filepath);

           for (int i = 1; i <= reader.NumberOfPages; i++)                   
            {
                text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
            }

            return text.ToString();
        }

    }
}
