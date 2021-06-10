using iText.Kernel.Pdf;
using System.IO;

namespace PdfPasswordRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change the values to the right ones.

            // The path where the original PDFs are located.
            string mainPath = @"c:\pathToPDFsWithPassword";
            // The path where do you want to save the decrypted PDFs. I use a subfolder in the same path.
            string decryptedFilesPath = Path.Combine(mainPath, "NowDecrypted");
            // The password of the PDFs.
            var password = "MySuperSecretPassword";

            // Start the process.
            var files = Directory.GetFiles(mainPath, "*.pdf");
            var pdfManipulator = new PdfManipulation();

            foreach (var file in files)
            {
                var dest = Path.Combine(decryptedFilesPath, Path.GetFileName(file));
                pdfManipulator.ManipulatePdf(file, dest, password);
            }
        }

        public class PdfManipulation
        {
            public void ManipulatePdf(string src, string dest, string password)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(password);

                var reader = new MyReader(src, passwordBytes);
                reader.SetUnethicalReading(true);
                reader.DecryptOnPurpose();

                if (!Directory.Exists(Path.GetDirectoryName(dest)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                }

                var pdfDoc = new PdfDocument(reader, new PdfWriter(dest));
                pdfDoc.Close();
                reader.Close();
            }
        }

        class MyReader : PdfReader
        {
            public MyReader(string filename, byte[] passwordBytes) : base(filename, new ReaderProperties().SetPassword(passwordBytes)) { }
            public void DecryptOnPurpose()
            {
                encrypted = false;
            }
        }
    }
}
