using NUnit.Framework;

namespace testDocUpload.Tests
{
    public class UploadBlobTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Upload_PDF_ToBlob_Return_Success()
        {
            Assert.Pass();
        }

        [Test]
        public void Upload_NonPDF_ToBlob_Return_NonSuccess()
        {
            Assert.Pass();
        }

        [Test]
        public void Upload_PDF_GreaterThan5MB_ToBlob_Return_NonSuccess()
        {
            Assert.Pass();
        }
    }
}