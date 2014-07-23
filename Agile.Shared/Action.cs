
namespace Agile.Shared
{
    /// <summary>
    /// For some reason SL doesn't provide a non generic
    /// delegate. 
    /// </summary>
    /// <remarks>File should only be included in the SL project.
    /// Am confused by (non generic) Action, in SL it doesn't show in intellisense
    /// and resharper show red for compile error, but it compiles no problem?
    /// </remarks>
    public delegate void Action();

}
