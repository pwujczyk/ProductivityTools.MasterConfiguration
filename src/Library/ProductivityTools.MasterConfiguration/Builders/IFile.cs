namespace ProductivityTools.MasterConfiguration.Builders
{
    interface IFile : IBuilder
    {
        SourceType SourceType { get; }
    }
}