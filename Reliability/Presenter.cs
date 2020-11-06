using MainViewInterface;
using ModelInterface;
using Reliability;
using System.Xml;

namespace Presenter
{
    public class Presenter
    {
        IMainView View;
        IModel Model;
        public Presenter(IMainView View, IModel Model)
        {
            this.View = View;
            this.Model = Model;
            View.GetReliabilityReport += View_GetReliabilityReport;
            View.GetGystogramm += View_GetGystogramm;
            View.GetSystemXmlOutlet += View_GetSystemXmlOutlet;
            View.LoadSystemStructure += View_LoadSystemStructure;
            View.LoadInputData += View_LoadInputData;
            View.LoadExample += View_LoadExample;
            View.SaveInputData += View_SaveInputData;
            Model.SuccessCalculationProgressChanged += Model_SuccessCalculationProgressChanged;
            Model.GystogrammCalculationProgressChanged += Model_GystogrammCalculationProgressChanged;
        }

        void Model_SuccessCalculationProgressChanged(object sender, SimulationProgressChangedEventArgs e)
        {
            View.SetSuccessProbabilityCalculationProgress(e.ProgressState);
        }

        private void Model_GystogrammCalculationProgressChanged(object sender, SimulationProgressChangedEventArgs e)
        {
            View.SetGystogrammCalculationProgress(e.ProgressState);
        }

        void View_SaveInputData(object sender, SaveInputDataEventArgs e)
        {
            Model.SaveInputData(e.FileName, e.inputData);
        }

        void View_LoadSystemStructure(object sender, LoadSystemStructureEventArgs e)
        {
            e.systemStructure = Model.LoadSystemStructure(e.FileName);
        }

        void View_LoadInputData(object sender, LoadInputDataEventArgs e)
        {
            e.inputData = Model.LoadInputData(e.FileName);
        }

        private void View_LoadExample(object sender, LoadExampleEventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ExampleResource.Example);
            e.inputData = Model.LoadInputData(xmlDoc);
        }

        void View_GetSystemXmlOutlet(object sender, GetSystemXmlOutletEventArgs e)
        {
            e.SystemStructureXmlOutlet = Model.GetSystemXmlOutlet(e.systemStructure);
        }

        void View_GetReliabilityReport(object sender, GetReliabilityReportEventArgs e)
        {
            Model.SetStructure(e.systemStructure);
            e.reliabilityReport = Model.GetReliabilityReport(e.SimulationCount, e.Time);
        }
        void View_GetGystogramm(object sender, GetGystogrammEventArgs e)
        {
            Model.SetStructure(e.systemStructure);
            e.gystogramm = Model.GetGystogramm(e.SimulationCount, e.InitialTime);
        }
    }
}
