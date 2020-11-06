using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Types;
using MainViewInterface;
using System.IO;
using ZedGraph;
using System.Drawing;

namespace Reliability
{
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
            systemStructure = new SystemStructure();
            elementPropertiesForm = new ElementPropertiesForm();
            elementPropertiesForm.ElementPropertiesApplied += elementPropertiesForm_ElementPropertiesApplied;
            GystogrammChart.GraphPane.IsFontsScaled = false;
            GystogrammChart.GraphPane.Title.IsVisible = false;
            GystogrammChart.GraphPane.XAxis.Title.Text = "Время, ч";
            GystogrammChart.GraphPane.YAxis.Title.Text = "Число отказов";
            GystogrammChart.GraphPane.XAxis.MajorGrid.IsVisible = true;
            GystogrammChart.GraphPane.YAxis.MajorGrid.IsVisible = true;
            GystogrammChart.GraphPane.XAxis.Scale.Min = 0;
            GystogrammChart.GraphPane.YAxis.Scale.Min = 0;
            GystogrammChart.GraphPane.Border.IsVisible = false;
        }
        struct SelectedElement
        {
            public Element element;
            public GroupOfElements parent;
            public int index;
            public SelectedElement(Element element, GroupOfElements parent, int index)
            {
                this.element = element;
                this.parent = parent;
                this.index = index;
            }
        }
        private void elementPropertiesForm_ElementPropertiesApplied(object sender, ElementPropertiesAppliedEventArgs e)
        {
            SelectedElement selectedElement = GetSystemStructureElement(currentTreeNode);
            selectedElement.parent.ChildElements[selectedElement.index] = e.systemElement;
            currentTreeNode.Text = e.systemElement.Name;
            RefreshRichTextBox();
        }

        private ElementPropertiesForm elementPropertiesForm;
        private SystemStructure systemStructure;
        private TreeNode currentTreeNode;
        private Gystogramm gystogramm;
        private SelectedElement GetSystemStructureElement(TreeNode Node)
        {
            GroupOfElements parent = null;
            int index = -1;
            Element element;
            if (Node == StructureTreeView.Nodes[0])
                element = systemStructure.Root;
            else
            {
                List<int> pass = new List<int>();
                while (Node != StructureTreeView.Nodes[0])
                {
                    pass.Add(Node.Index);
                    Node = Node.Parent;
                }
                pass.Reverse();
                parent = systemStructure.Root;
                for (int i = 0; i < pass.Count - 1; i++)
                    parent = parent.ChildElements[pass[i]] as GroupOfElements;
                index = pass.Last();
                element = parent.ChildElements[index];
            }
            return new SelectedElement(element, parent, index);
        }
        
        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            string Name = "Новый элемент";
            StructureTreeView.SelectedNode.Nodes.Add(Name);
            SelectedElement selectedElement = GetSystemStructureElement(StructureTreeView.SelectedNode);
            Element ChildElement = new ElementWithGivenDurability();
            ChildElement.Name = Name;
            if (selectedElement.element.Type != SystemStructureElementType.GroupOfElements)
                selectedElement.parent.ChildElements[selectedElement.index] = new GroupOfElements(selectedElement.element.Name, 0, new List<Element>() { ChildElement });
            else
                (selectedElement.element as GroupOfElements).ChildElements.Add(ChildElement);
            RefreshRichTextBox();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StructureTreeView.SelectedNode != StructureTreeView.Nodes[0])
            {
                SelectedElement selectedElement = GetSystemStructureElement(StructureTreeView.SelectedNode);
                selectedElement.parent.ChildElements.RemoveAt(selectedElement.index);
                StructureTreeView.SelectedNode.Remove();
                if ((selectedElement.parent as GroupOfElements).ChildElements.Count == 0)
                {
                    SelectedElement parentElement = GetSystemStructureElement(StructureTreeView.SelectedNode);
                    Element ChildElement = new ElementWithGivenDurability();
                    ChildElement.Name = parentElement.element.Name;
                    parentElement.parent.ChildElements[parentElement.index] = ChildElement;
                }
                RefreshRichTextBox();
            }
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StructureTreeView.SelectedNode == StructureTreeView.Nodes[0]) return;
            currentTreeNode = StructureTreeView.SelectedNode;
            SelectedElement selectedElement = GetSystemStructureElement(currentTreeNode);
            elementPropertiesForm.CustomShow(selectedElement.element);
        }

        async private void SuccessProbabilityCalculateButton_Click(object sender, EventArgs e)
        {
            Model.Model model = new Model.Model();
            var eventArgs = new GetReliabilityReportEventArgs(
                systemStructure, 
                (int)Convert.ToDouble(SimulationCountForSuccessProbabilityCalculationTextBox.Text), 
                Convert.ToDouble(TimeForSuccessProbabilityCalculationTextBox.Text));
            await Task.Run(() => GetReliabilityReport(this, eventArgs));
            var report = eventArgs.reliabilityReport;
            var ElementNames = systemStructure.IndividualElements.Select(x => x.Name).ToList();
            SuccessProbabilityDataGridView.Rows.Clear();
            SuccessProbabilityDataGridView.RowCount = ElementNames.Count;
            for (int i = 0; i < ElementNames.Count; i++)
            {
                SuccessProbabilityDataGridView.Rows[i].Cells[0].Value = ElementNames[i];
                SuccessProbabilityDataGridView.Rows[i].Cells[1].Value = report.ElementsFailures[i];
                SuccessProbabilityDataGridView.Rows[i].Cells[2].Value = report.SystemFailuresConcernedWithElements[i];
                SuccessProbabilityDataGridView.Rows[i].Cells[3].Value =  (report.SystemFailuresConcernedWithElements[i] != 0) ?
                    report.SystemFailuresConcernedWithElements[i] * 100.0 / report.ElementsFailures[i] : 0;
            };
            SystemSuccessProbabilityTextBox.Text = report.SystemSuccessPrabability.ToString();
            SystemFailureCountTextBox.Text = report.SystemFailures.ToString();
        }

        async private void GystogrammCalculationButton_Click(object sender, EventArgs e)
        {
            var eventArgs = new GetGystogrammEventArgs(
                systemStructure,
                (int)Convert.ToDouble(GystogrammSimulationCountTextBox.Text),
                Convert.ToDouble(GystogrammInitialTimeTextBox.Text));
            await Task.Run(() => GetGystogramm(this, eventArgs));
            gystogramm = eventArgs.gystogramm;
            GystogrammDataGridView.Rows.Clear();
            GystogrammDataGridView.Columns.Clear();
            var ElementNames = systemStructure.IndividualElements.Select(x => x.Name).ToList();
            GystogrammCheckedListBox.Items.Clear();
            GystogrammCheckedListBox.Items.Add("Система");
            foreach (var elementName in ElementNames)
                GystogrammCheckedListBox.Items.Add(elementName);
            GystogrammDataGridView.ColumnCount = 2 + ElementNames.Count;
            GystogrammDataGridView.Columns[0].HeaderText = "Время, ч";
            GystogrammDataGridView.Columns[0].Frozen = true;
            for (int i = 0; i < ElementNames.Count; i++)
                GystogrammDataGridView.Columns[i + 1].HeaderText = "Число отказов элемента '" + ElementNames[i] + "'";
            GystogrammDataGridView.Columns[GystogrammDataGridView.ColumnCount - 1].HeaderText = "Число оказов системы";
            GystogrammDataGridView.RowCount = gystogramm.Times.Count;
            for (int i = 0; i < gystogramm.Times.Count; i++)
            {
                GystogrammDataGridView.Rows[i].Cells[0].Value = gystogramm.Times[i];
                for (int j = 0; j < ElementNames.Count; j++)
                    GystogrammDataGridView.Rows[i].Cells[j + 1].Value = gystogramm.ElementsFailures[j][i];
                GystogrammDataGridView.Rows[i].Cells[GystogrammDataGridView.ColumnCount - 1].Value = gystogramm.SystemFailures[i];
            }
            GystogrammDataGridView.Sort(GystogrammDataGridView.Columns[GystogrammDataGridView.ColumnCount - 1], ListSortDirection.Descending);
        }

        private void GystogrammCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = GystogrammCheckedListBox.SelectedIndex;
            if ((SelectedIndex == -1) || (gystogramm == null)) return;
            for (int i = 0; i < GystogrammCheckedListBox.Items.Count; i++)
                GystogrammCheckedListBox.SetItemChecked(i, false);
            GystogrammCheckedListBox.SetItemChecked(SelectedIndex, true);
            var Pane = GystogrammChart.GraphPane;
            Pane.CurveList.Clear();
            double[] x = gystogramm.Times.ToArray();
            double[] y = (SelectedIndex == 0 ?
                gystogramm.SystemFailures :
                gystogramm.ElementsFailures[SelectedIndex - 1]).Select(p => (double)p).ToArray();
            BarItem curve = Pane.AddBar("", x, y, Color.Blue);
            curve.Bar.Border.Color = Color.Blue;
            GystogrammChart.AxisChange();
            GystogrammChart.Invalidate();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                 SaveInputData(this, new SaveInputDataEventArgs(
                     fileDialog.FileName, 
                     new InputData(
                        systemStructure,
                        Convert.ToInt32(SimulationCountForSuccessProbabilityCalculationTextBox.Text),
                        Convert.ToDouble(TimeForSuccessProbabilityCalculationTextBox.Text),
                        Convert.ToInt32(GystogrammSimulationCountTextBox.Text),
                        Convert.ToDouble(GystogrammInitialTimeTextBox.Text))));
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var eventArgs = new LoadInputDataEventArgs(openDialog.FileName);
                LoadInputData(this, eventArgs);
                var inputData = eventArgs.inputData;
                systemStructure = inputData.SystemStructure;
                SimulationCountForSuccessProbabilityCalculationTextBox.Text = inputData.SimulationCountForSucessProbabilityCalculation.ToString();
                TimeForSuccessProbabilityCalculationTextBox.Text = inputData.TimeForSuccessProbabilityCalculation.ToString();
                GystogrammSimulationCountTextBox.Text = inputData.GystogrammSimulationCount.ToString();
                GystogrammInitialTimeTextBox.Text = inputData.GystogrammInitialTime.ToString();
                RefreshStructureTreeView();
                RefreshRichTextBox();
            }
        }

        private void RefreshRichTextBox()
        {
            XmlRichTextBox.Text = "";
            var eventArgs = new GetSystemXmlOutletEventArgs(systemStructure);
            GetSystemXmlOutlet(this, eventArgs);
            var sb = eventArgs.SystemStructureXmlOutlet;
            XmlRichTextBox.AppendText(sb.ToString(0, sb.Length));
            XmlRichTextBox.Text = XmlRichTextBox.Text.Remove(XmlRichTextBox.GetFirstCharIndexFromLine(0), XmlRichTextBox.Lines[0].Length + 1);
        }

        private void RefreshStructureTreeView()
        {
            StructureTreeView.Nodes.Clear();
            Action<Element, TreeNode> AddTreeNodes = null;
            AddTreeNodes = (Element, TreeNode) =>
            {
                if (Element.Type != SystemStructureElementType.GroupOfElements)
                    TreeNode.Text = Element.Name;
                else
                {
                    var element = Element as GroupOfElements;
                    if (element != systemStructure.Root)
                        TreeNode.Text = element.Name;
                    else
                        TreeNode.Text = "Структурная схема";
                    for (int i = 0; i < element.ChildElements.Count; i++)
                    {
                        var currentElement = element.ChildElements[i];
                        var currentTreeNode = new System.Windows.Forms.TreeNode();
                        AddTreeNodes(currentElement, currentTreeNode);
                        TreeNode.Nodes.Add(currentTreeNode);
                    }
                }
            };
            TreeNode RootTreeNode = new System.Windows.Forms.TreeNode();
            AddTreeNodes(systemStructure.Root, RootTreeNode);
            StructureTreeView.Nodes.Add(RootTreeNode);
            StructureTreeView.ExpandAll();
        }

        private void XmlRenewMenuItem_Click(object sender, EventArgs e)
        {
            string FileName = "tmp.xml";
            File.WriteAllText(FileName, XmlRichTextBox.Text);
            try
            {
                var eventArgs = new LoadSystemStructureEventArgs(FileName);
                LoadSystemStructure(this, eventArgs);
                systemStructure = eventArgs.systemStructure;
                RefreshStructureTreeView();
            }
            catch 
            {
                RefreshRichTextBox();
            }
            finally
            {
                File.Delete(FileName);
            }
        }

        private void ExampleMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new LoadExampleEventArgs();
            LoadExample(this, eventArgs);
            var inputData = eventArgs.inputData;
            systemStructure = inputData.SystemStructure;
            SimulationCountForSuccessProbabilityCalculationTextBox.Text = inputData.SimulationCountForSucessProbabilityCalculation.ToString();
            TimeForSuccessProbabilityCalculationTextBox.Text = inputData.TimeForSuccessProbabilityCalculation.ToString();
            GystogrammSimulationCountTextBox.Text = inputData.GystogrammSimulationCount.ToString();
            GystogrammInitialTimeTextBox.Text = inputData.GystogrammInitialTime.ToString();
            RefreshStructureTreeView();
            RefreshRichTextBox();
            SuccessProbabilityCalculateButton_Click(this, EventArgs.Empty);
            GystogrammCalculationButton_Click(this, EventArgs.Empty);
        }
        public void SetSuccessProbabilityCalculationProgress(int ProgressState)
        {
            Invoke(new Action(() => SuccessCalculationProgressBar.Value = Math.Min(ProgressState, 100)));
        }
        public void SetGystogrammCalculationProgress(int ProgressState)
        {
            Invoke(new Action(() => GystogrammCalculationProgressBar.Value = Math.Min(ProgressState, 100)));
        }
        public event EventHandler<SaveInputDataEventArgs> SaveInputData;
        public event EventHandler<LoadSystemStructureEventArgs> LoadSystemStructure;
        public event EventHandler<LoadInputDataEventArgs> LoadInputData;
        public event EventHandler<LoadExampleEventArgs> LoadExample;
        public event EventHandler<GetSystemXmlOutletEventArgs> GetSystemXmlOutlet;
        public event EventHandler<GetReliabilityReportEventArgs> GetReliabilityReport;
        public event EventHandler<GetGystogrammEventArgs> GetGystogramm;
    }
}
