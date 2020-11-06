using System;
using System.Windows.Forms;
using Types;

namespace Reliability
{
    public partial class ElementPropertiesForm : Form
    {
        public ElementPropertiesForm()
        {
            InitializeComponent();
            TopPanelHeight = NamePanel.Height;
            MiddlePanleHeight = GroupOfElementsPanel.Height;
            BottomPanelHeight = ChildElementPanel.Height;
            FormHeight = Height;
        }
        int TopPanelHeight;
        int MiddlePanleHeight;
        int BottomPanelHeight;
        int FormHeight;
        Element element;
        public void CustomShow(Element element)
        {
            this.element = element;
            NameTextBox.Text = element.Name;
            if (element.Type == SystemStructureElementType.GroupOfElements)
            {
                GroupOfElementsPanel.Visible = true;
                ChildElementPanel.Visible = false;
                Height = FormHeight - BottomPanelHeight;
                MajorityTextBox.Text = (element as GroupOfElements).Majority.ToString();
            }
            else
            {
                GroupOfElementsPanel.Visible = false;
                ChildElementPanel.Visible = true;
                Height = FormHeight - MiddlePanleHeight;
                CyclogramGrid.Rows.Clear();
                if (element.Type == SystemStructureElementType.ElementWithGivenSuccessProbability)
                {
                    SuccessProbabilityRadioButton.Checked = true;
                    SuccessPossibilityTextBox.Text = (element as ElementWithGivenSuccessProbability).SuccessProbability.ToString();
                    CycleDurationTextBox.Text = (element as ElementWithGivenSuccessProbability).CycleDuration.ToString();
                    DurabilityTextBox.Text = "";
                    CyclogramGrid.Columns[0].HeaderText = "Время работы, ч";
                    CyclogramGrid.Columns[1].Visible = false;
                    double[] Cyclogramm = (element as ChildElement).Cyclogramm;
                    CyclogramGrid.RowCount = Cyclogramm.Length;
                    for (int i = 0; i < CyclogramGrid.RowCount; i++)
                        CyclogramGrid.Rows[i].Cells[0].Value = Cyclogramm[i];
                }
                else
                {
                    DurabilityRadioButton.Checked = true;
                    DurabilityTextBox.Text = (element as ElementWithGivenDurability).Durability.ToString();
                    CycleDurationTextBox.Text = (element as ElementWithGivenDurability).CycleDuration.ToString();
                    SuccessPossibilityTextBox.Text = "";
                    CyclogramGrid.Columns[0].HeaderText = "Начало интервала, ч";
                    CyclogramGrid.Columns[1].HeaderText = "Конец интервала, ч";
                    CyclogramGrid.Columns[1].Visible = true;
                    double[] Cyclogramm = (element as ChildElement).Cyclogramm;
                    CyclogramGrid.RowCount = Cyclogramm.Length / 2;
                    for (int i = 0; i < CyclogramGrid.RowCount; i++)
                    {
                        CyclogramGrid.Rows[i].Cells[0].Value = Cyclogramm[2 * i];
                        CyclogramGrid.Rows[i].Cells[1].Value = Cyclogramm[2 * i + 1];
                    }
                }
                ElementTypeRadioButton_CheckedChanged(this, EventArgs.Empty);
            }
            Show();
        }

        private double[] GetCyclogramm(SystemStructureElementType Type)
        {
            if (Type == SystemStructureElementType.ElementWithGivenSuccessProbability)
            {
                double[] Cyclogramma = new double[CyclogramGrid.RowCount];
                for (int i = 0; i < CyclogramGrid.RowCount; i++)
                    Cyclogramma[i] = Convert.ToDouble(CyclogramGrid.Rows[i].Cells[0].Value.ToString());
                return Cyclogramma;
            }
            else
            {
                double[] Cyclogramma = new double[2 * CyclogramGrid.RowCount];
                for (int i = 0; i < CyclogramGrid.RowCount; i++)
                {
                    Cyclogramma[2 * i] = Convert.ToDouble(CyclogramGrid.Rows[i].Cells[0].Value.ToString());
                    Cyclogramma[2 * i + 1] = Convert.ToDouble(CyclogramGrid.Rows[i].Cells[1].Value.ToString());
                }
                return Cyclogramma;
            }
        }
        
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            element.Name = NameTextBox.Text;
            if (element.Type != SystemStructureElementType.GroupOfElements)
            {
                if (SuccessProbabilityRadioButton.Checked)
                    element = new ElementWithGivenSuccessProbability(NameTextBox.Text, GetCyclogramm(element.Type), Convert.ToDouble(SuccessPossibilityTextBox.Text), Convert.ToDouble(CycleDurationTextBox.Text));
                else
                    element = new ElementWithGivenDurability(NameTextBox.Text, GetCyclogramm(element.Type), Convert.ToDouble(DurabilityTextBox.Text), Convert.ToDouble(CycleDurationTextBox.Text));
            }
            else
                (element as GroupOfElements).Majority = Convert.ToDouble(MajorityTextBox.Text);
            ElementPropertiesApplied(this, new ElementPropertiesAppliedEventArgs(element));
        }

        private void ElementPropertiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ElementTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SuccessPossibilityTextBox.Enabled = SuccessProbabilityRadioButton.Checked;
            DurabilityTextBox.Enabled = !SuccessPossibilityTextBox.Enabled;
        }

        public event EventHandler<ElementPropertiesAppliedEventArgs> ElementPropertiesApplied;

        private void ModifyRowCount_Click(object sender, EventArgs e)
        {
            if (sender == AddRowButton)
            {
                if (CyclogramGrid.RowCount == 0)
                    CyclogramGrid.Rows.Add();
                else
                {
                    int index = CyclogramGrid.CurrentRow.Index;
                    if (index != CyclogramGrid.RowCount - 1)
                        CyclogramGrid.Rows.Insert(index + 1, 1);
                    else
                        CyclogramGrid.Rows.Add();
                }
            }
            else if (sender == RemoveRowButton)
            {
                if (CyclogramGrid.RowCount != 0)
                    CyclogramGrid.Rows.RemoveAt(CyclogramGrid.CurrentRow.Index);
            }
            else
                CyclogramGrid.Rows.Clear();
        }
    }

    public class ElementPropertiesAppliedEventArgs : EventArgs
    {
        public Element systemElement { get; private set; }
        public ElementPropertiesAppliedEventArgs(Element systemElement)
        {
            this.systemElement = systemElement;
        }
    }
}