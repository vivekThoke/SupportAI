using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace SupportAI.Infrastructure.Processing
{
    public class TextExtractor
    {
        public string Extract(string filePath)
        {
            if (filePath.EndsWith(".pdf"))
            {
                using var document = PdfDocument.Open(filePath);
                var text = "";

                foreach(var page in document.GetPages())
                {
                    text += page.Text;
                }

                return text;
            }

            return File.ReadAllText(filePath);
        }
    }
}
