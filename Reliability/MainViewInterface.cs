using System;
using System.Text;
using Types;

namespace MainViewInterface
{
    public interface IMainView
    {
        void SetSuccessProbabilityCalculationProgress(int ProgressState);
        void SetGystogrammCalculationProgress(int ProgressState);
        event EventHandler<SaveInputDataEventArgs> SaveInputData;
        event EventHandler<LoadSystemStructureEventArgs> LoadSystemStructure;
        event EventHandler<LoadInputDataEventArgs> LoadInputData;
        event EventHandler<LoadExampleEventArgs> LoadExample;
        event EventHandler<GetSystemXmlOutletEventArgs> GetSystemXmlOutlet;
        event EventHandler<GetReliabilityReportEventArgs> GetReliabilityReport;
        event EventHandler<GetGystogrammEventArgs> GetGystogramm;
    }
    public class GetSystemXmlOutletEventArgs : EventArgs
    {
        public SystemStructure systemStructure { get; private set; }
        public StringBuilder SystemStructureXmlOutlet { get; set; }
        public GetSystemXmlOutletEventArgs(SystemStructure systemStructure)
        {
            this.systemStructure = systemStructure;
        }
    }
    public class GetReliabilityReportEventArgs : EventArgs
    {
        public SystemStructure systemStructure { get; private set; }
        public int SimulationCount { get; private set; }
        public double Time { get; private set; }
        public ReliabilityReport reliabilityReport { get; set; }
        public GetReliabilityReportEventArgs(SystemStructure systemStructure, int SimulationCount, double Time)
        {
            this.systemStructure = systemStructure;
            this.SimulationCount = SimulationCount;
            this.Time = Time;
        }
    }
    public class GetGystogrammEventArgs : EventArgs
    {
        public SystemStructure systemStructure { get; private set; }
        public int SimulationCount { get; private set; }
        public double InitialTime { get; private set; }
        public Gystogramm gystogramm { get; set; }
        public GetGystogrammEventArgs(SystemStructure systemStructure, int SimulationCount, double InitialTime)
        {
            this.systemStructure = systemStructure;
            this.SimulationCount = SimulationCount;
            this.InitialTime = InitialTime;
        }
    }
    public class LoadInputDataEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public InputData inputData { get; set; }
        public LoadInputDataEventArgs(string FileName)
        {
            this.FileName = FileName;
        }
    }
    public class LoadExampleEventArgs
    {
        public InputData inputData { get; set; }
    }
    public class LoadSystemStructureEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public SystemStructure systemStructure { get; set; }
        public LoadSystemStructureEventArgs(string FileName)
        {
            this.FileName = FileName;
        }
    }
    public class SaveInputDataEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public InputData inputData { get; private set; }
        public SaveInputDataEventArgs(string FileName, InputData inputData)
        {
            this.FileName = FileName;
            this.inputData = inputData;
        }
    }
}
