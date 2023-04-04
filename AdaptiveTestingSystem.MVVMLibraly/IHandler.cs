

namespace AdaptiveTestingSystem.MVVMLibraly
{
    public interface IHandler
    {
        public delegate void OverlayShowingHandler(bool show);
        public event OverlayShowingHandler? OverlayShowing;

        public delegate void OverlayChangeInformationHandler(string firstValue, string lastValue);
        public event OverlayChangeInformationHandler? OverlayChangeInformation;

        public delegate void UpdateHandler(bool skipCheck);
        public event UpdateHandler? Update;

        public delegate void DeleteEventHandler(System.Collections.IList deleteList);
        public event DeleteEventHandler? DeleteObjects;
    }
}
