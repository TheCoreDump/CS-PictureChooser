using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace PictureChooser
{
    public class PictureProcessor : IDisposable
    {
        private DirectoryInfo workDirectory = new DirectoryInfo("C:\\Temp\\PictureWorkSpace");

        public PictureProcessor()
        {
            if (!workDirectory.Exists)
                workDirectory.Create();

            makeTempCopyFileBlock = new TransformBlock<FileInfo, FileInfo>((fi) => MakeTempCopy(fi), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 3 });
            finalBlock = new ActionBlock<FileInfo>((fi) => Debug.WriteLine($"Process of file {fi.FullName} Complete"));

            makeTempCopyFileBlock.LinkTo(finalBlock, new DataflowLinkOptions() { PropagateCompletion = true });
        }

        TransformBlock<FileInfo, FileInfo> makeTempCopyFileBlock;
        ActionBlock<FileInfo> finalBlock;

        public void ProcessPhoto(FileInfo fileInfo)
        {
            makeTempCopyFileBlock.Post(fileInfo);
        }

        protected FileInfo MakeTempCopy(FileInfo fileInfo)
        {
            string newFileName = Path.Combine(workDirectory.FullName, fileInfo.Name);
            return fileInfo.CopyTo(newFileName);
        }

        protected FileInfo RotateImage(FileInfo imageFile)
        {
            using (FileStream FS = imageFile.Open(FileMode.Open, FileAccess.Read))
            {
                Image i = Image.FromFile("test");

                Console.WriteLine($"Image Size: {i.Size.Width} x {i.Size.Height}");
            }


        }

        protected void DeleteTempCopy(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
                fileInfo.Delete();
        }

        public void Dispose()
        {
            makeTempCopyFileBlock.Complete();

            finalBlock.Completion.Wait();
        }
    }
}
