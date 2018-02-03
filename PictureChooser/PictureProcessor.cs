using System;
using System.Diagnostics;
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

        public void Dispose()
        {
            makeTempCopyFileBlock.Complete();

            finalBlock.Completion.Wait();
        }
    }
}
