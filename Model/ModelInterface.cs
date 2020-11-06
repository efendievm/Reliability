using System;
using System.Text;
using System.Xml;
using Types;

namespace ModelInterface
{
    public interface IModel
    {
        ReliabilitySimulation SimulateSystemReliability(int SimulationCount, double Time);
        ReliabilitySimulation SimulateSystemCurrentReliability(int SimulationCount, double Time);
        ReliabilityReport GetReliabilityReport(int SimulationCount, double Time);
        ReliabilityReport GetCurrentReliabilityReport(int SimulationCount, double Time);
        Gystogramm GetGystogramm(int SimulationCount, double InitialTime);
        void SetStructure(SystemStructure systemStructure);
        SystemStructure LoadSystemStructure(string FileName);
        InputData LoadInputData(XmlDocument xmlDoc);
        InputData LoadInputData(string FileName);
        void SaveInputData(string FileName, InputData inputData);
        StringBuilder GetSystemXmlOutlet(SystemStructure systemStructure);
        event EventHandler<SimulationProgressChangedEventArgs> SuccessCalculationProgressChanged;
        event EventHandler<SimulationProgressChangedEventArgs> GystogrammCalculationProgressChanged;
    }
    public class SimulationProgressChangedEventArgs : EventArgs
    {
        public int ProgressState { get; private set; }
        public SimulationProgressChangedEventArgs(int ProgressState)
        {
            this.ProgressState = ProgressState;
        }
    }
}